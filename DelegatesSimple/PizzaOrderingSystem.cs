using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesSimple {
    class PizzaOrderingSystem {
        readonly DiscountPolicy _discountPolicy;

        public PizzaOrderingSystem(DiscountPolicy discountPolicy) {
            _discountPolicy = discountPolicy;
        }

        public decimal ComputePrice(PizzaOrder order) {
            decimal nonDiscounted = order.Pizzas.Sum(p => p.Price);
            decimal discountedValue = _discountPolicy(order);
            return nonDiscounted - discountedValue;
        }
    }

    public class PizzaOrder {
        public List<Pizza> Pizzas { get; set; }
        public PizzaOrder() {
            Pizzas = new List<Pizza>();
        }
    }

    public enum Size {
        Small, Medium, Large
    }

    public enum Crust {
        Thin, Regular, Stuffed
    }

    public class Pizza {
        public Size Size { get; set; }
        public Crust Crust { get; set; }
        public decimal Price { get; set; }
    }

    public delegate decimal DiscountPolicy(PizzaOrder order);

    public static class DiscountPolicyDelegates {
        public static decimal BuyOneGetOneFree(PizzaOrder order) {
            var pizzas = order.Pizzas;
            if (pizzas.Count < 2) {
                return 0m;
            }
            return pizzas.Min(p => p.Price);
        }
        public static decimal FivePercentOffMoreThanFiftyDollars(PizzaOrder order) {
            decimal nonDiscounted = order.Pizzas.Sum(p => p.Price);
            return nonDiscounted >= 50 ? nonDiscounted*0.05m : 0M;
        }
        public static decimal FiveDollarsOffStuffedCrust(PizzaOrder order) {
            return order.Pizzas.Sum(p => p.Crust == Crust.Stuffed ? 5m : 0m);
        }
        public static DiscountPolicy CreateBest(params DiscountPolicy[] policies) {
            return order => policies.Max(policy => policy.Invoke(order));
        }

        public static DiscountPolicy DiscountAllThePizzas() {
            DiscountPolicy best = CreateBest(
                BuyOneGetOneFree,
                FivePercentOffMoreThanFiftyDollars,
                FiveDollarsOffStuffedCrust);
            return best;
        }
    }
}
