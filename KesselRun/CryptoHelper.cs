using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KesselRun
{
    class CryptoHelper
    {
        private static byte[] PuzzleAnswer = { 0x81, 0xa1, 0x95, 0x70, 0xd1, 0xfa, 0x6c, 0x9d,
                                               0x8f, 0xe8, 0x7a, 0x6b, 0x90, 0x73, 0x62, 0xa5 };
        private static byte[] PuzzleIV = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                                           0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        public static string GetAnswer(List<Direction> moveSequence)
        {
            byte[] key = MoveSequenceToKey(moveSequence);

            AesManaged aes = new AesManaged();
            aes.Key = key;
            aes.IV = PuzzleIV;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] output = new byte[PuzzleAnswer.Length];
            decryptor.TransformBlock(PuzzleAnswer, 0, PuzzleAnswer.Length, output, 0);

            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            return encoding.GetString(output);
        }

        public static byte[] CreateEncryptedAnswerBlob(string answer, List<Direction> moveSequence)
        {
            byte[] key = MoveSequenceToKey(moveSequence);

            AesManaged aes = new AesManaged();
            aes.Key = key;
            aes.IV = PuzzleIV;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            byte[] answerBuffer = encoding.GetBytes(answer);

            byte[] encAnswer = new byte[answerBuffer.Length];
            encryptor.TransformBlock(answerBuffer, 0, answerBuffer.Length, encAnswer, 0);

            return encAnswer;
        }

        private static byte[] MoveSequenceToKey(List<Direction> moveSequence)
        {
            byte[] translatedMoves = new byte[moveSequence.Count];
            for (int i = 0; i < moveSequence.Count; i++)
            {
                switch (moveSequence[i])
                {
                    case Direction.Left:
                        translatedMoves[i] = 0x01;
                        break;
                    case Direction.UpLeft:
                        translatedMoves[i] = 0x02;
                        break;
                    case Direction.UpRight:
                        translatedMoves[i] = 0x03;
                        break;
                    case Direction.Right:
                        translatedMoves[i] = 0x04;
                        break;
                    case Direction.DownRight:
                        translatedMoves[i] = 0x05;
                        break;
                    case Direction.DownLeft:
                        translatedMoves[i] = 0x06;
                        break;
                    default:
                        throw new ArgumentException("Unknown direction");
                }
            }

            byte[] key = new SHA256Managed().ComputeHash(translatedMoves);
            return key;
        }
    }
}
