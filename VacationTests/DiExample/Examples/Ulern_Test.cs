using DiExample.Examples.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DiExample.Examples;

public class Ulern_Test
{
    [Test]
    public void DiTest_AddScoped_And_Singleton_UseCommonToken()
    {
        // должен использоваться общий токен
        var container = new ServiceCollection()
            .AddSingleton<Token>()
            .AddScoped<ConsoleTokenWriter1>()
            .AddScoped<ConsoleTokenWriter2>()
            .BuildServiceProvider();

        var sp1 = container.CreateScope().ServiceProvider;
        var sp2 = container.CreateScope().ServiceProvider;

        Assert.AreEqual(
            sp1.GetRequiredService<ConsoleTokenWriter1>().TokenInfo,
            sp2.GetRequiredService<ConsoleTokenWriter1>().TokenInfo,
            "Должен быть одинаковый токен в разных скоупах"
        );
    }

    [Test]
    public void DiTest_AllAddScoped_UseRandomToken()
    {
        //используем разные токены для разных скоупов
        var container = new ServiceCollection()
            .AddScoped<Token>()
            .AddScoped<ConsoleTokenWriter1>()
            .AddScoped<ConsoleTokenWriter2>()
            .AddScoped<DoubleToken>()
            .BuildServiceProvider();

        var sp1 = container.CreateScope().ServiceProvider;
        var sp2 = container.CreateScope().ServiceProvider;

        var instance_1_1 = sp1.GetRequiredService<ConsoleTokenWriter1>();
        var instance_2_1 = sp2.GetRequiredService<ConsoleTokenWriter1>();
        var instance_2_2 = sp2.GetRequiredService<ConsoleTokenWriter2>();
        var instanceDoubleToken = sp2.GetRequiredService<DoubleToken>();


        Assert.Multiple(() =>
            {
                Assert.AreNotEqual(instance_1_1.TokenInfo, instance_2_1.TokenInfo,
                    "в разных СКОУПАХ для 1 сервиса должны быть разные токены, т.к. токен добавлен через AddScoped");
                Assert.AreEqual(instance_2_1.TokenInfo, instance_2_2.TokenInfo,
                    "для разных сервисах внутри общего скоупа должен быть общий токен, т.к. токен добавлен через AddScoped");
                Assert.AreNotEqual(instance_1_1.TokenInfo, instance_2_2.TokenInfo,
                    "для разных сервисах внутри разных скоупов должны быть разные токены, т.к. токен добавлен через AddScoped");
                Assert.AreEqual(instanceDoubleToken.Token1, instanceDoubleToken.Token2,
                    "Для сервиса принимаеющего на вход 2 токена должен использоваться 1 общий из скоупа");
            }
        );
    }

    [Test]
    public void DiTest_AddTransientToken()
    {
        //используем разные токены 
        var container = new ServiceCollection()
            .AddTransient<Token>()
            .AddScoped<ConsoleTokenWriter1>()
            .AddScoped<ConsoleTokenWriter2>()
            .AddScoped<DoubleToken>()
            .BuildServiceProvider();

        var sp1 = container.CreateScope().ServiceProvider;
        var sp2 = container.CreateScope().ServiceProvider;

        var instance_1_1 = sp1.GetRequiredService<ConsoleTokenWriter1>();
        var instance_2_1 = sp2.GetRequiredService<ConsoleTokenWriter1>();
        var instance_2_2 = sp2.GetRequiredService<ConsoleTokenWriter2>();
        var instanceDoubleToken = sp2.GetRequiredService<DoubleToken>();

        Assert.Multiple(() =>
            {
                Assert.AreNotEqual(instance_1_1.TokenInfo, instance_2_1.TokenInfo,
                    "в разных СКОУПАХ для 1 сервиса должны быть разные токены, т.к. токен добавлен через AddTransient");
                Assert.AreNotEqual(instance_2_1.TokenInfo, instance_2_2.TokenInfo,
                    "для разных сервисах внутри общего скоупа должены быть разные токены, т.к. токен добавлен через AddTransient");
                Assert.AreNotEqual(instance_1_1.TokenInfo, instance_2_2.TokenInfo,
                    "для разных сервисах внутри разных скоупов должны быть разные токены, т.к. токен добавлен через AddTransient");
                Assert.AreNotEqual(instanceDoubleToken.Token1, instanceDoubleToken.Token2,
                    "Для сервиса принимаеющего на вход 2 токена должены сгенерироваться 2 разных токена");
            }
        );
    }
}