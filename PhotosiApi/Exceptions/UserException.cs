﻿namespace PhotosiApi.Exceptions;

public class UserException : Exception
{
    public UserException()
    {
    }

    public UserException(string message) : base(message)
    {
    }
}