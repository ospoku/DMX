using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.DTO.Memos
{
   public class AssignMemoDTO
    {
      public int   MemoId{get; set;}
        public string MemoName{get; set;} = string.Empty;
        public string MemoType{get; set;}=string.Empty;
        public List<string> SelectedUserIds {  get; set;} = new List<string>();
    }
}
