using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Cromulent.Encoding.Tests
{
    [TestClass]
    public class Z85Tests
    {
        /// <summary>
        /// From the ZeroMQ RFC - https://rfc.zeromq.org/spec:32/Z85/
        /// As a test case, a frame containing these 8 bytes: 0x86 | 0x4F | 0xD2 | 0x6F | 0xB5 | 0x59 | 0xF7 | 0x5B
        /// SHALL encode as the following 10 characters: HelloWorld
        /// </summary>
        [TestMethod]
        public void ToZ85String_HelloWorld_Success()
        {
            var bytes = new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
            var expectedOutput = "HelloWorld";

            var output = Z85.ToZ85String(bytes);

            Assert.AreEqual(expectedOutput, output);
        }

        /// <summary>
        /// From the ZeroMQ RFC - https://rfc.zeromq.org/spec:32/Z85/
        /// As a test case, a frame containing these 8 bytes: 0x86 | 0x4F | 0xD2 | 0x6F | 0xB5 | 0x59 | 0xF7 | 0x5B
        /// SHALL encode as the following 10 characters: HelloWorld
        /// </summary>
        [TestMethod]
        public void FromZ85String_HelloWorld_Success()
        {
            var encodedString = "HelloWorld";
            var expectedOutput = new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };

            var output = Z85.FromZ85String(encodedString);

            CollectionAssert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void EncodeDecode_NoPaddingRequired_Success()
        {
            var input = "RiAZ3bax"; // 8 bytes - divisible by 4 with no remainder
            var inputBytes = System.Text.Encoding.Default.GetBytes(input);

            var outputBytes = Z85.FromZ85String(Z85.ToZ85String(inputBytes));

            var output = System.Text.Encoding.Default.GetString(outputBytes);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void EncodeDecode_PaddingRequired_Success()
        {
            var input = "HelloWorld"; // 10 bytes - NOT divisible by 4 with no remainder
            var inputBytes = System.Text.Encoding.Default.GetBytes(input);

            var outputBytes = Z85.FromZ85String(Z85.ToZ85String(inputBytes, true));

            var output = System.Text.Encoding.Default.GetString(outputBytes);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void ToZ85String_NoPaddingRequired_OutputSizeIsCorrect()
        {
            var input = "ABCD1234";
            var inputBytes = System.Text.Encoding.Default.GetBytes(input);
            var expectedEncodeSize = 10; // 4 bytes become 5

            var output = Z85.ToZ85String(inputBytes);

            Assert.AreEqual(expectedEncodeSize, output.Length);
        }

        [TestMethod]
        public void ToZ85String_PaddingRequired_OutputSizeIsCorrect()
        {
            var input = "HelloWorld";
            var inputBytes = System.Text.Encoding.Default.GetBytes(input);
            var expectedEncodeSize = 15 + 1; // 4 bytes become 5, so with padding HelloWorld is 15 bytes encoded (+1 for padding value)

            var output = Z85.ToZ85String(inputBytes, true);

            Assert.AreEqual(expectedEncodeSize, output.Length);
        }

        /// <summary>
        /// From the ZeroMQ RFC - https://rfc.zeromq.org/spec:32/Z85/
        /// The binary frame SHALL have a length that is divisible by 4 with no remainder.
        /// </summary>
        [TestMethod]
        public void FromZ85String_OutputSizeIsCorrect()
        {
            var encodedText = "ABCDE12345";
            var expectedDecodeSize = 8; // 5 characters decode to 4 bytes

            var output = Z85.FromZ85String(encodedText);

            Assert.AreEqual(expectedDecodeSize, output.Length);
        }

        /// <summary>
        /// From the ZeroMQ RFC - https://rfc.zeromq.org/spec:32/Z85/
        /// The string frame SHALL have a length that is divisible by 5 with no remainder.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromZ85String_InputSizeIncorrect_ThrowsException()
        {
            var encodedText = "WrongSize"; // Length - 1 not divisible by 5

            var output = Z85.FromZ85String(encodedText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromZ85String_PaddingCharacterNotDigit_ThrowsException()
        {
            var encodedText = "IllegalChrA"; // Padding char must be 0, 1, 2 or 3

            var output = Z85.FromZ85String(encodedText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromZ85String_PaddingCharacterIncorrectValue_ThrowsException()
        {
            var encodedText = "InvalidChr4"; // Padding char must be 0, 1, 2 or 3

            var output = Z85.FromZ85String(encodedText);
        }
    }
}