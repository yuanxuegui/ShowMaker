using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using ShowMaker.Desktop.Models.Util;

namespace ShowMakerTest
{
    /// <summary>
    /// ShowFileEncryptDecriptTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ShowFileEncryptDecriptTest
    {
        public ShowFileEncryptDecriptTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            /*
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            DES.IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            byte[] endata = desencrypt.TransformFinalBlock(data, 0, 5);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            byte[] dedata = desdecrypt.TransformFinalBlock(endata, 0, 8);
            System.Diagnostics.Debug.WriteLine("" + dedata[2]);
            */
            // 文件加密
            byte[] keyBytes = new byte[] { 0x2D, 0x6D, 0x28, 0x5E, 0x2E, 0x20, 0x23, 0x2C };
		    string textFile = "D:/test.txt";
		    string destFile = "D:/test.csdes";
		    string decryptedFile = destFile + ".csdecrypted";
            ShowFileEncryptDecrypt.EncryptFile(textFile, destFile, ASCIIEncoding.ASCII.GetString(keyBytes));
            ShowFileEncryptDecrypt.DecryptFile(destFile, decryptedFile, ASCIIEncoding.ASCII.GetString(keyBytes));
        }
    }
}
