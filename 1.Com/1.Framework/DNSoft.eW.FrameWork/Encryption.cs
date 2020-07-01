using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
 
namespace DNSoft.eW.FrameWork
{
    #region Encryptor 암호화객체
    /// <summary>
    /// <b>대칭형 암호 객체를 인스턴스하여, 데이터를 암호화 한다.</b><br/>
    /// </summary>
	public class Encryptor
	{
 		private EncryptTransformer transformer;
		private byte[] initVec;
		private byte[] encKey;

        /// <summary>
        /// 암호화 객체 생성<br/>
         /// </summary>
        /// <param name="algId">암호화알고리즘ID</param>
		public Encryptor(EncryptionAlgorithm algId)
		{
			transformer = new EncryptTransformer(algId);
		}

        /// <summary>
        /// 대칭형 암호화알고리즘에 사용하는 백터값<br/>
        /// </summary>
		public byte[] IV
		{
			get{return initVec;}
			set{initVec = value;}
		}

        /// <summary>
        /// 대칭형 암호화알고리즘에 사용하는 키값<br/>
        /// </summary>
		public byte[] Key
		{
			get{return encKey;}
		}

        /// <summary>
        /// 데이터를 암호화한다.
        /// </summary>
        /// <param name="bytesData">암호할 데이터</param>
        /// <param name="bytesKey">암호화키</param>
        /// <returns>암호화한 데이터</returns>
        /// <example>
        /// 암호 키와 백터, 암호할 데이터를 바이트로 변환하여 암호화 객체를 생성한다.
        /// <code>
        ///     byte[] key = Encoding.UTF8.GetBytes(암호키값);
        ///     byte[] IV = Encoding.UTF8.GetBytes(암호백터값);
        ///     byte[] cipherText = null;
        ///     Encryptor enc = new Encryptor(algorithm);    
		///     enc.IV = IV;
		///     cipherText = enc.Encrypt(Encoding.UTF8.GetBytes(암호할데이터), key);    
        /// </code>
        /// </example>
		public byte[] Encrypt(byte[] bytesData, byte[] bytesKey)
		{
			//Set up the stream that will hold the encrypted data.
			MemoryStream memStreamEncryptedData = new MemoryStream();
			transformer.IV = initVec;
			ICryptoTransform transform = transformer.GetCryptoServiceProvider(bytesKey);
			CryptoStream encStream = new CryptoStream(memStreamEncryptedData,
				transform,
				CryptoStreamMode.Write);
			try
			{
				//Encrypt the data, write it to the memory stream.
				encStream.Write(bytesData, 0, bytesData.Length);
			}
			catch(Exception ex)
			{
				throw new Exception("Error while writing encrypted data to the stream: \n"
					+ ex.Message);
			}
			//Set the IV and key for the client to retrieve
			encKey = transformer.Key;
			initVec = transformer.IV;
			encStream.FlushFinalBlock();
			encStream.Close();
			//Send the data back.
			return memStreamEncryptedData.ToArray();
		}//end Encrypt

    }
    #endregion 

    #region Decryptor 복호화 객체
    /// <summary>
    /// <b>대칭형 암호 객체를 인스턴스하여, 데이터를 복호화 한다.</b><br/>
    /// </summary>
	public class Decryptor
	{
		private DecryptTransformer transformer;
		private byte[] initVec;

        /// <summary>
        /// 복호화 객체<br/>
        /// </summary>
        /// <param name="algId">암호화알고리즘ID</param>
		public Decryptor(EncryptionAlgorithm algId)
		{
			transformer = new DecryptTransformer(algId);
		}

        /// <summary>
        /// 대칭형 암호화알고리즘에 사용하는 백터값<br/>
        /// </summary>
		public byte[] IV
		{
			set{initVec = value;}
		}

        /// <summary>
        /// 데이터를 복호화한다.</br>
        /// </summary>
        /// <param name="bytesData">복호화할 데이터</param>
        /// <param name="bytesKey">암호화키</param>
        /// <returns>복호화된 데이터</returns>
        /// <example>
        /// 암호 키와 백터, 복호할 데이터를 바이트로 변환하여 복호화 객체를 생성한다.
        /// <code>
        ///     byte[] key = Encoding.UTF8.GetBytes(암호키값);
        ///     byte[] IV = Encoding.UTF8.GetBytes(암호백터값);
        ///     Decryptor dec = new Decryptor(algorithm);   
        ///     dec.IV = IV;
        ///     byte[] encryptText = Convert.FromBase64String(복호할데이터);
        ///     byte[] plainText = dec.Decrypt(encryptText, key);  
        /// </code>
        /// </example>
		public byte[] Decrypt(byte[] bytesData, byte[] bytesKey)
		{
			//Set up the memory stream for the decrypted data.
			MemoryStream memStreamDecryptedData = new MemoryStream();
			//Pass in the initialization vector.
			transformer.IV = initVec;
			ICryptoTransform transform = transformer.GetCryptoServiceProvider(bytesKey);
			CryptoStream decStream = new CryptoStream(memStreamDecryptedData,
				transform,
				CryptoStreamMode.Write);
			try
			{
				decStream.Write(bytesData, 0, bytesData.Length);
			}
			catch(Exception ex)
			{
				throw new Exception("Error while writing encrypted data to the stream: \n"
					+ ex.Message);
			}
			decStream.FlushFinalBlock();
			decStream.Close();
			// Send the data back.
			return memStreamDecryptedData.ToArray();
		} //end Decrypt

    }
    #endregion 

    #region EncryptionAlgorithm - 대칭형암호화 알고리즘 타입 : 열거형
    /// <summary>
    /// 대칭형 암호화알고리즘 타입<br/>
    /// </summary>
	public enum EncryptionAlgorithm {Des = 1, Rc2, Rijndael, TripleDes};
    #endregion 

    #region EncryptTransformer
    /// <summary>
    ///   암호화 객체를 인스턴스화 한다<br/>
    /// </summary>
	internal class EncryptTransformer
	{
		private EncryptionAlgorithm algorithmID;
		private byte[] initVec;
		private byte[] encKey;

        /// <summary>
        /// 암호화 알고리즘 객체 인스턴스 생성자<br/>
        /// </summary>
        /// <param name="algId">암호화알고리즘ID</param>
		internal EncryptTransformer(EncryptionAlgorithm algId)
		{
			//Save the algorithm being used.
			algorithmID = algId;
		}

        /// <summary>
        /// 대칭형 암호화알고리즘에 사용하는 백터값<br/>
        /// </summary>
		internal byte[] IV
		{
			get{return initVec;}
			set{initVec = value;}
		}

        /// <summary>
        /// 대칭형 암호화알고리즘에 사용하는 키값<br/>
        /// </summary>
		internal byte[] Key
		{
			get{return encKey;}
		}

        /// <summary>
        /// 암호화 객체를 생성한다.
        /// </summary>
        /// <param name="bytesKey">암호키</param>
        /// <returns>생성된 객체</returns>
		internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
		{
			try
			{
				// Pick the provider.
				switch (algorithmID)
				{
					case EncryptionAlgorithm.Des:
					{
						DES des = new DESCryptoServiceProvider();
						des.Mode = CipherMode.CBC;
						// See if a key was provided
						if (null == bytesKey)
						{
							encKey = des.Key;
						}
						else
						{
							des.Key = bytesKey;
							encKey = des.Key;
						}
						// See if the client provided an initialization vector
						if (null == initVec)
						{ // Have the algorithm create one
							initVec = des.IV;
						}
						else
						{ //No, give it to the algorithm
							des.IV = initVec;
						}
						return des.CreateEncryptor();
					}
					case EncryptionAlgorithm.TripleDes:
					{
						TripleDES des3 = new TripleDESCryptoServiceProvider();
						des3.Mode = CipherMode.CBC;
						// See if a key was provided
						if (null == bytesKey)
						{
							encKey = des3.Key;
						}
						else
						{
							des3.Key = bytesKey;
							encKey = des3.Key;
						}
						// See if the client provided an IV
						if (null == initVec)
						{ //Yes, have the alg create one
							initVec = des3.IV;
						}
						else
						{ //No, give it to the alg.
							des3.IV = initVec;
						}
						return des3.CreateEncryptor();
					}
					case EncryptionAlgorithm.Rc2:
					{
						RC2 rc2 = new RC2CryptoServiceProvider();
						rc2.Mode = CipherMode.CBC;
						// Test to see if a key was provided
						if (null == bytesKey)
						{
							encKey = rc2.Key;
						}
						else
						{
							rc2.Key = bytesKey;
							encKey = rc2.Key;
						}
						// See if the client provided an IV
						if (null == initVec)
						{ //Yes, have the alg create one
							initVec = rc2.IV;
						}
						else
						{ //No, give it to the alg.
							rc2.IV = initVec;
						}
						return rc2.CreateEncryptor();
					}
					case EncryptionAlgorithm.Rijndael:
					{
						Rijndael rijndael = new RijndaelManaged();
						rijndael.Mode = CipherMode.CBC;
						// Test to see if a key was provided
						if(null == bytesKey)
						{
							encKey = rijndael.Key;
						}
						else
						{
							rijndael.Key = bytesKey;
							encKey = rijndael.Key;
						}
						// See if the client provided an IV
						if(null == initVec)
						{ //Yes, have the alg create one
							initVec = rijndael.IV;
						}
						else
						{ //No, give it to the alg.
							rijndael.IV = initVec;
						}
						return rijndael.CreateEncryptor();
					}
					default:
					{
						throw new CryptographicException("Algorithm ID '" + algorithmID +
							"' not supported.");
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

    }
    #endregion

    #region DecryptTransformer
    /// <summary>
    /// 복호화 객체를 인스턴스화 한다<br/>
    /// </summary>
    internal class DecryptTransformer
	{
		private EncryptionAlgorithm algorithmID;
		private byte[] initVec;


        /// <summary>
        /// 암호화 알고리즘 객체 인스턴스 생성자<br/>
        /// </summary>
        /// <param name="algId">암호화알고리즘ID</param>
		internal DecryptTransformer(EncryptionAlgorithm deCryptId)
		{
			algorithmID = deCryptId;
		}

        /// <summary>
        /// 대칭형 암호화알고리즘에 사용하는 백터값<br/>
        /// </summary>
		internal byte[] IV
		{
			set{initVec = value;}
		}

        /// <summary>
        /// 복호화 객체를 생성한다.
        /// </summary>
        /// <param name="bytesKey">암호키</param>
        /// <returns>생성된 객체</returns>
		internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
		{
			try
			{
				// Pick the provider.
				switch (algorithmID)
				{
					case EncryptionAlgorithm.Des:
					{
						DES des = new DESCryptoServiceProvider();
						des.Mode = CipherMode.CBC;
						des.Key = bytesKey;
						des.IV = initVec;
						return des.CreateDecryptor();
					}
					case EncryptionAlgorithm.TripleDes:
					{
						TripleDES des3 = new TripleDESCryptoServiceProvider();
						des3.Mode = CipherMode.CBC;
						return des3.CreateDecryptor(bytesKey, initVec);
					}
					case EncryptionAlgorithm.Rc2:
					{
						RC2 rc2 = new RC2CryptoServiceProvider();
						rc2.Mode = CipherMode.CBC;
						return rc2.CreateDecryptor(bytesKey, initVec);
					}
					case EncryptionAlgorithm.Rijndael:
					{
						Rijndael rijndael = new RijndaelManaged();
						rijndael.Mode = CipherMode.CBC;
						return rijndael.CreateDecryptor(bytesKey, initVec);
					}
					default:
					{
						throw new CryptographicException("Algorithm ID '" + algorithmID +
							"' not supported.");
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

		} //end GetCryptoServiceProvider

    }
    #endregion
 
 
}
