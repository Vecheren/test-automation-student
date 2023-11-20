// using System;
// using System.Threading.Tasks;
// using Microsoft.Extensions.DependencyInjection;
// using NUnit.Framework;
//
// namespace DiExample;
//
// [SetUpFixture]
// public class SetUpContainer
// {
//     public static readonly IServiceProvider ContainerCache
//         = Container.BuildServiceProvider();
//         
//     [OneTimeTearDown]
//     public async Task TearDown()
//         => await (ContainerCache as ServiceProvider)!.DisposeAsync();
// }