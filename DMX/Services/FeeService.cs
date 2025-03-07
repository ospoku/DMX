using DMX.Data;
using DMX.Models;
using System;
using System.Linq;

namespace DMX.Services
{
    public class FeeService
    {
        private readonly XContext _context;

        public FeeService(XContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Calculates the fee based on the number of days and the deceased type (e.g., died in ward or brought in dead).
        /// </summary>
        /// <param name="numberOfDays">The number of days the deceased stayed.</param>
        /// <param name="deceasedTypeId">The ID of the deceased type (e.g., 1 = died in ward, 2 = brought in dead).</param>
        /// <returns>The calculated fee.</returns>
        public decimal FeeCalculator(int numberOfDays, string deceasedTypeId, List<DeceasedService> selectedServices)
        {
            decimal totalFee = 0;

           // Fetch the fee structures based on the deceased type ID
            var feeStructures = _context.FeeStructures
                .Where(f => f.DeceasedTypeId == deceasedTypeId) // Use the foreign key to match fee structures
                .OrderBy(f => f.MinDays)
                .ToList();

            if (!feeStructures.Any())
            {
                throw new InvalidOperationException("No fee structures found for the specified deceased type.");
            }

            foreach (var tier in feeStructures)
            {
                if (numberOfDays > tier.MaxDays)
                {
                   // Calculate fee for the entire tier

                   totalFee += (tier.MaxDays - tier.MinDays + 1) * tier.Fee;
                }
                else if (numberOfDays >= tier.MinDays)
                {
                   // Calculate fee for the remaining days within the tier

                   totalFee += (numberOfDays - tier.MinDays + 1) * tier.Fee;
                    return totalFee;
                }
            }

            //Handle days beyond the last tier
            var lastTier = feeStructures.LastOrDefault();
            if (lastTier != null && numberOfDays > lastTier.MaxDays)
            {
                totalFee += (numberOfDays - lastTier.MaxDays) * lastTier.Fee;
            }


            //Add charges for selected extra services
            if (selectedServices != null && selectedServices.Any())
                {
                    foreach (var service in selectedServices)
                    {
                        totalFee += service.MorgueService.Amount;
                    }
                }


            return totalFee;
        }






        //public class FeeService
        //{
        //    private readonly XContext _context;

        //    public FeeService(XContext context)
        //    {
        //        _context = context;
        //    }

        //    // Existing method for ViewInvoice
        //    public List<TierCharge> FeeCalculator(int numberOfDays, string deceasedTypeId, List<MorgueService> selectedServices)
        //    {
        //        // Existing logic for ViewInvoice
        //    }

        //    // New method for ViewDeceaseds
        //    public List<TierCharge> CalculateDeceasedFees(int numberOfDays, string deceasedTypeId, int numberOfServices, decimal totalServiceAmount)
        //    {
        //        var tierCharges = new List<TierCharge>();

        //        // Fetch the fee structures based on the deceased type ID
        //        var feeStructures = _context.FeeStructures
        //            .Where(f => f.DeceasedTypeId == deceasedTypeId)
        //            .OrderBy(f => f.MinDays)
        //            .ToList();

        //        if (!feeStructures.Any())
        //        {
        //            throw new InvalidOperationException("No fee structures found for the specified deceased type.");
        //        }

        //        // Calculate tier charges based on the number of days
        //        foreach (var tier in feeStructures)
        //        {
        //            if (numberOfDays > tier.MaxDays)
        //            {
        //                // Calculate fee for the entire tier
        //                var daysCharged = tier.MaxDays - tier.MinDays + 1;
        //                var totalCharge = daysCharged * tier.Fee;

        //                tierCharges.Add(new TierCharge
        //                {
        //                    MinDays = tier.MinDays,
        //                    MaxDays = tier.MaxDays,
        //                    DaysCharged = daysCharged,
        //                    Fee = tier.Fee,
        //                    TotalCharge = totalCharge
        //                });

        //                numberOfDays -= daysCharged;
        //            }
        //            else if (numberOfDays >= tier.MinDays)
        //            {
        //                // Calculate fee for the remaining days within the tier
        //                var daysCharged = numberOfDays - tier.MinDays + 1;
        //                var totalCharge = daysCharged * tier.Fee;

        //                tierCharges.Add(new TierCharge
        //                {
        //                    MinDays = tier.MinDays,
        //                    MaxDays = tier.MaxDays,
        //                    DaysCharged = daysCharged,
        //                    Fee = tier.Fee,
        //                    TotalCharge = totalCharge
        //                });

        //                break; // Exit the loop after processing the current tier
        //            }
        //        }

        //        // Add charges for the number of services
        //        if (numberOfServices > 0)
        //        {
        //            tierCharges.Add(new TierCharge
        //            {
        //                MinDays = 0,
        //                MaxDays = 0,
        //                DaysCharged = 0,
        //                Fee = numberOfServices * 10, // Example: $10 per service
        //                TotalCharge = numberOfServices * 10
        //            });
        //        }

        //        // Add the total amount of selected services
        //        if (totalServiceAmount > 0)
        //        {
        //            tierCharges.Add(new TierCharge
        //            {
        //                MinDays = 0,
        //                MaxDays = 0,
        //                DaysCharged = 0,
        //                Fee = totalServiceAmount,
        //                TotalCharge = totalServiceAmount
        //            });
        //        }

        //        return tierCharges;
        //    }
        //}






    }
}