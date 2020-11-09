using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature
{
    public class RNGService
    {
        private Random r = new Random();
        public async Task<string> GetRandomSequence(int sequenceLength)
        {
            return await Task.Run(() =>
            {
                if (sequenceLength > 0)
                {
                    List<bool> flips = new List<bool>();
                    for (int i = 0; i < sequenceLength; i++)
                    {
                        flips.Add(r.Next(0, 2) == 1);
                    }
                    var yellowMoon = new Emoji("🌝");
                    var blueMoon = new Emoji("🌚");
                    string result = string.Join(" ", flips.Select((item) => item ? yellowMoon : blueMoon));
                    return result;
                }
                return $"Sequence length must be positive but {sequenceLength} given.";
            });
        }
    }
}
