using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CipherGame.Models
{
    public class TeamStateModel
    {
        public string Message { get; set; }
        public string CipherCode { get; set; }
        public bool IsPlaceFound { get; set; }
        public string TeamName { get; set; }
    }
}
