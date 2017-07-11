using System;
using System.Collections.Generic;
using Twilio;

namespace VA.Service

{
    public class SMSService
    {
        public void Send(string phoneNumber, string name, string message)
        {
            // set our AccountSid and AuthToken
            string AccountSid = "AC25195d70ba44c681b9b24a78ea22a4ff";
            string AuthToken = "9f9059fd2c66671cf5bc6be265946c4e";

            // instantiate a new Twilio Rest Client
            var client = new TwilioRestClient(AccountSid, AuthToken);
            // make an associative array of people we know, indexed by phone number
            var result = client.SendMessage("412-413-9257", phoneNumber, string.Format("Hello {0}, tomorrow you have appointment with us! " + message, name));

        }
    }
}