using Guni_Kitchen.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Guni_Kitchen.Data
{
    public class ApplicationDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Product.Any())
                {
                    context.Product.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            ProductName = "Margherita",
                            Description = "A hugely popular margherita, with a deliciously tangy single cheese topping",
                            Price = 19.50,
                            ImageURL = "https://www.dominos.co.in/files/items/Margherit.jpg",
                            ProductCategory = ProductCategory.Pizza
                        },
                        new Product()
                        {
                            ProductName = "Farm House",
                            Description = "Overload of crunchy, crisp capsicum, succulent mushrooms and fresh tomatoes",
                            Price = 29.50,
                            ImageURL = "https://www.dominos.co.in/files/items/Farmhouse.jpg",
                            ProductCategory = ProductCategory.Pizza
                        },
                        new Product()
                        {
                            ProductName = "Spinach & Corn",
                            Description = "Made with spinach, sweet corn and garlic and choice of dips.",
                            Price = 35.50,
                            ImageURL = "https://res.cloudinary.com/swiggy/image/upload/fl_lossy,f_auto,q_auto,w_1024/skgbvyv4ssuofssj7g8r",
                            ProductCategory = ProductCategory.Burger
                        },
                        new Product()
                        {
                            ProductName = "Veg Burger",
                            Description = "Crispy veg patty, crunchy lettuce, tomato, onion and mayonnaise.",
                            Price = 27.50,
                            ImageURL = "https://res.cloudinary.com/swiggy/image/upload/fl_lossy,f_auto,q_auto,w_1024/c1jbtuj4hansmcxrv3sd",
                            ProductCategory = ProductCategory.Burger
                        },
                        new Product()
                        {
                            ProductName = "Peppy Paneer",
                            Description = "Chunky paneer with crisp capsicum and spicy red pepper - quite a mouthful!",
                            Price = 45.50,
                            ImageURL = "https://www.dominos.co.in/files/items/Peppy_Paneer.jpg",
                            ProductCategory = ProductCategory.Pizza
                        },
                        new Product()
                        {
                            ProductName = "Cheesy Peri Peri Paneer",
                            Description = "Fresh paneer chunks, onion, piri piri spread, mozzarella cheese.",
                            Price = 39.50,
                            ImageURL = "https://res.cloudinary.com/swiggy/image/upload/fl_lossy,f_auto,q_auto,w_1024/dl9rldzmjzivsllswwxv",
                            ProductCategory = ProductCategory.Burger
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
