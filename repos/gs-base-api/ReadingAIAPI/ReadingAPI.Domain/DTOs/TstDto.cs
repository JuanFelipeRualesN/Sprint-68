using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingAPI.ReadingAPI.Domain.DTOs
{
    public class TtsWord
    {
        public double Offset { get; set; }    // seconds from clip start
        public double Duration { get; set; }  // seconds
        public string Text { get; set; } = "";
    }

    public class TtsResult
    {
        public byte[] Audio { get; set; } = Array.Empty<byte>();
        public List<TtsWord> Words { get; set; } = new();
    }
}
