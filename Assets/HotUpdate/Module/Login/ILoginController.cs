using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ILoginController : IController
{
    void Login(string acc, string pass);
}

[Controller(typeof(ILoginController))]
public class LoginController : ILoginController
{
    public void Login(string acc, string pass)
    {

    }
}