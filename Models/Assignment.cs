using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLearningApp.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int ClassID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }

}
