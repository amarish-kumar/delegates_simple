using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace DelegatesSimple {

    public class PizzaOrderingSystemInterfaceTests {
        private readonly ITestOutputHelper _output;

        public PizzaOrderingSystemInterfaceTests(ITestOutputHelper output) {
            _output = output;
        }

        
        [Fact]
        public void Get_a_pizza_no_discount() {
            var pizzaOrderingSystem = new PizzaOrderingSystemInterfaces(new BestDiscount());
            var order = new PizzaOrder() {
                Pizzas = new List<Pizza>() {
                    new Pizza() {
                        Crust = Crust.Regular,
                        Price = 10.00m,
                        Size = Size.Large
                    }
                }
            };
            var price = pizzaOrderingSystem.ComputePrice(order);
            Assert.Equal(price, 10.00m);
        }

        [Fact]
        public void Buy_one_get_one_policy() {
            var pizzaOrderingSystem = new PizzaOrderingSystemInterfaces(new BestDiscount());
            var order = new PizzaOrder();
            for (int i = 0; i < 2; i++) {
                var pizza = new Pizza() {
                    Crust = Crust.Regular,
                    Price = 10.00m,
                    Size = Size.Large
                };
                order.Pizzas.Add(pizza);
            }
            var price = pizzaOrderingSystem.ComputePrice(order);
            Assert.Equal(2, order.Pizzas.Count);
            Assert.Equal(10.00m, price);
        }

        [Fact]
        public void Five_percent_off_more_than_50_policy() {
            var pizzaOrderingSystem = new PizzaOrderingSystemInterfaces(new BestDiscount());
            var order = new PizzaOrder();
            for (int i = 0; i < 6; i++) {
                var pizza = new Pizza() {
                    Crust = Crust.Regular,
                    Price = 10.00m,
                    Size = Size.Large
                };
                order.Pizzas.Add(pizza);
            }
            var price = pizzaOrderingSystem.ComputePrice(order);
            Assert.Equal(6, order.Pizzas.Count);
            Assert.Equal(50.00m, price);
        }

        [Fact]
        public void Five_dollars_off_stuffed_crust() {
            var pizzaOrderingSystem = new PizzaOrderingSystemInterfaces(new BestDiscount());
            var order = new PizzaOrder() {
                Pizzas = new List<Pizza>() {
                    new Pizza() {
                        Crust = Crust.Stuffed,
                        Price = 10.00m,
                        Size = Size.Large
                    }
                }
            };
            var price = pizzaOrderingSystem.ComputePrice(order);
            Assert.Equal(price, 5.00m);
        }

        [Fact]
        public void Best_discount_for_big_order() {
            var pizzaOrderingSystem = new PizzaOrderingSystemInterfaces(new BestDiscount());
            var order = new PizzaOrder();
            // Buy one get one
            for (int i = 0; i < 2; i++) {
                var pizza = new Pizza() {
                    Crust = Crust.Regular,
                    Price = 10.00m,
                    Size = Size.Large
                };
                order.Pizzas.Add(pizza);
            }
            // Over 50 5% off
            for (int i = 0; i < 6; i++) {
                var pizza = new Pizza() {
                    Crust = Crust.Regular,
                    Price = 10.00m,
                    Size = Size.Large
                };
                order.Pizzas.Add(pizza);
            }
            // Stuffed crust
            var stuffedCrust = new Pizza() {
                Crust = Crust.Stuffed,
                Price = 10.00m,
                Size = Size.Large
            };
            order.Pizzas.Add(stuffedCrust);
            var price = pizzaOrderingSystem.ComputePrice(order);
            Assert.Equal(80.00m, price);
        }
    }

}