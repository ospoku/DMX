using DMX.Data;
using DMX.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Constraint = DMX.Models.Constraint;

namespace DMX.Services
{
    public class TimetableGenerator
    {
        private readonly XContext _context;

        public TimetableGenerator(XContext context)
        {
            _context = context;
        }

        public List<TimeTableEntry> GenerateTimetable()
        {
            var constraints = _context.Constraints.ToList();
            var subjects = _context.Subjects.Include(s => s.Groups).ToList();
            var teachers = _context.Teachers.ToList();
            var classrooms = _context.Classrooms.ToList();
            var timeSlots = _context.TimeSlots.ToList();
            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();

            var timetable = new List<TimeTableEntry>();
            var random = new Random();

            foreach (var subject in subjects)
            {
                // Divide students into groups if necessary
                var groups = DivideStudentsIntoGroups(subject, classrooms);

                foreach (var group in groups)
                {
                    foreach (var day in days)
                    {
                        foreach (var timeSlot in timeSlots)
                        {
                            var availableTeachers = teachers
                                .Where(t => !timetable.Any(entry => entry.TeacherId == t.TeacherId && entry.TimeSlotId == timeSlot.TimeSlotId && entry.Day == day))
                                .ToList();

                            var availableClassrooms = classrooms
                                .Where(c => !timetable.Any(entry => entry.Group.ClassroomId == c.ClassroomId && entry.TimeSlotId == timeSlot.TimeSlotId && entry.Day == day))
                                .ToList();

                            // Apply runtime constraints (e.g., teacher availability)
                            availableTeachers = ApplyTeacherAvailabilityConstraints(availableTeachers, constraints, day);

                            if (availableTeachers.Any() && availableClassrooms.Any())
                            {
                                var teacher = availableTeachers[random.Next(availableTeachers.Count)];
                                var classroom = availableClassrooms[random.Next(availableClassrooms.Count)];

                                // Assign the classroom to the group
                                group.ClassroomId = classroom.Id;

                                timetable.Add(new TimeTableEntry
                                {
                                    GroupId = group.Id,
                                    TeacherId = teacher.TeacherId,
                                    TimeSlotId = timeSlot.TimeSlotId,
                                    Day = day
                                });

                                break; // Move to the next group
                            }
                        }
                    }
                }
            }

            return timetable;
        }

        private List<Group> DivideStudentsIntoGroups(Subject subject, List<Classroom> classrooms)
        {
            var groups = new List<Group>();
            var remainingStudents = subject.StudentPopulation;
            var classroomCapacity = classrooms.Min(c => c.Capacity); // Use the smallest classroom capacity

            while (remainingStudents > 0)
            {
                var groupSize = Math.Min(classroomCapacity, remainingStudents);
                var group = new Group
                {
                    SubjectId = subject.SubjectId,
                    StudentCount = groupSize
                };
                groups.Add(group);

                remainingStudents -= groupSize;
            }

            return groups;
        }

        private List<Teacher> ApplyTeacherAvailabilityConstraints(List<Teacher> teachers, List<Constraint> constraints, DayOfWeek day)
        {
            var availabilityConstraints = constraints
                .Where(c => c.Type == "TeacherAvailability")
                .Select(c => c.Value.Split(';'))
                .Where(parts => parts[1].Split('=')[1] == day.ToString())
                .ToList();

            foreach (var constraint in availabilityConstraints)
            {
                var teacherId = int.Parse(constraint[0].Split('=')[1]);
                var isAvailable = bool.Parse(constraint[2].Split('=')[1]);

                if (!isAvailable)
                {
                    teachers.RemoveAll(t => t.TeacherId == teacherId);
                }
            }

            return teachers;
        }
    }
}