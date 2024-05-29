﻿namespace RapidPay.Domain.Abstractions
{
    public interface ISecurityServices
    {
        string Tokenize(string input);
        string Encrypt(string input);
    }
}