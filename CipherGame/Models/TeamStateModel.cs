using System.Text;

namespace CipherGame.Models
{
    public class TeamStateModel
    {
        public string Message { get; set; }
        public string CipherCode { get; set; }
        public bool IsPlaceFound { get; set; }
        public string TeamName { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{{ teamName: {TeamName}");
            sb.Append($", cipherCode: {CipherCode}");
            sb.Append($", isPlaceFound: {IsPlaceFound}");
            sb.Append($", message: {Message} }}");

            return sb.ToString();
        }
    }
}
