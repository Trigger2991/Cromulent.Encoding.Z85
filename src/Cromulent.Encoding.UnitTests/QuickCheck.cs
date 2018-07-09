using FsCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cromulent.Encoding.Tests
{
    /// <summary>
    /// Random tests using FsCheck, a port of Haskell's QuickCheck.
    /// </summary>
    [TestClass]
    public class QuickCheck
    {
        [TestMethod]
        [Ignore]
        public void EncodeDecode_100Times_OutputMatchesOriginal()
        {
            Prop.ForAll<byte[]>(bytes =>
            {
                Z85.FromZ85String(Z85.ToZ85String(bytes, autoPad: true));
            }
            ).QuickCheckThrowOnFailure();
        }

        [TestMethod]
        [Ignore]
        public void EncodeDecode_1000Times_Max50Kb_OutputMatchesOriginal()
        {
            var config = Configuration.QuickThrowOnFailure;
            config.MaxNbOfTest = 1000;
            config.StartSize = 0;
            config.EndSize = 51200;

            Prop.ForAll<byte[]>(bytes =>
            {
                Z85.FromZ85String(Z85.ToZ85String(bytes, autoPad: true));
            }
            ).Check(config);
        }
    }
}