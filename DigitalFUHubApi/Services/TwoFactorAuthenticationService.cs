﻿using Google.Authenticator;
using System.Drawing;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.QrCode;

namespace DigitalFUHubApi.Services
{
	public class TwoFactorAuthenticationService
	{
		private readonly IConfiguration configuration;

		public TwoFactorAuthenticationService(IConfiguration _configuration)
		{
			configuration = _configuration;	
		}

		internal string GenerateSecretKey()
		{
			return Guid.NewGuid().ToString();
		}

		internal (string, string) GetAccountManualEntryKey(string secretKey, string userName)
		{
			var totp = new TwoFactorAuthenticator();
			var info = totp.GenerateSetupCode(configuration["JWT:Issuer"], userName, secretKey, false, 3);
			return (info.Account, info.ManualEntryKey);
		}

		internal string GenerateQrCode(string secretKey, string userName)
		{
			var totp = new TwoFactorAuthenticator();
			var setupInfo = totp.GenerateSetupCode(configuration["JWT:Issuer"], userName, secretKey,false,3);
			var qrCode = setupInfo.QrCodeSetupImageUrl;
			return qrCode;
		}

		internal bool ValidateTwoFactorPin(string secretKey, string pin)
		{
			var totp = new TwoFactorAuthenticator();
			var currentPin = totp.GetCurrentPIN(secretKey, false);
			return currentPin == pin;
		}
	}
}
