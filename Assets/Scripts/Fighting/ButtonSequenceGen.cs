using Random = System.Random;

namespace Fighting
{
    public class ButtonSequenceGen
    {
        public static char[] GenerateButtonSeq(string letters, int count)
        {
            var result = new char[count];
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var index = random.Next(0, letters.Length);
                result[i] = letters[index];
            }
            return result;
        }
    }
}
