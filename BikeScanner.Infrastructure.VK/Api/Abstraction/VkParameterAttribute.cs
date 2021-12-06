using System;

namespace BikeScanner.Infrastructure.VK.Api.Abstraction
{
    internal class VkParameterAttribute : Attribute
    {
        public string Name { get; set; }

        public VkParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
