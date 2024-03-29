﻿namespace ITLearning.Domain;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
}