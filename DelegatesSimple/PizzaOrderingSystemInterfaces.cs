using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesSimple {
    class PizzaOrderingSystemInterfaces {
        readonly IDiscountPolicy _discountPolicy;
        public PizzaOrderingSystemInterfaces(IDiscountPolicy discountPolicy) {
            _discountPolicy = discountPolicy;
        }
        public decimal ComputePrice(PizzaOrder order) {
            decimal nonDiscounted = order.Pizzas.Sum(p => p.Price);
            decimal discountedValue = _discountPolicy.ApplyDiscount(order);
            return nonDiscounted - discountedValue;
        }
    }

    public interface IDiscountPolicy {
        decimal ApplyDiscount(PizzaOrder order);
    }

    public class BuyOneGetOneFree : IDiscountPolicy {
        public decimal ApplyDiscount(PizzaOrder order) {
            var pizzas = order.Pizzas;
            if (pizzas.Count < 2) {
                return 0m;
            }
            return pizzas.Min(p => p.Price);
        }
    }

    public class FivePercentOffMoreThanFiftyDollars : IDiscountPolicy {
        public decimal ApplyDiscount(PizzaOrder order) {
            decimal nonDiscounted = order.Pizzas.Sum(p => p.Price);
            return nonDiscounted >= 50 ? nonDiscounted * 0.05m : 0M;
        }
    }

    public class FiveDollarsOffStuffedCrust : IDiscountPolicy {
        public decimal ApplyDiscount(PizzaOrder order) {
            return order.Pizzas.Sum(p => p.Crust == Crust.Stuffed ? 5m : 0m);
        }
    }

    public class BestDiscount : IDiscountPolicy {
        public decimal ApplyDiscount(PizzaOrder order) {
            var discountPolicies = new List<IDiscountPolicy>() {
                new BuyOneGetOneFree(),
                new FivePercentOffMoreThanFiftyDollars(),
                new FiveDollarsOffStuffedCrust()
            };

            var bestDiscount = 
                discountPolicies.Max(policy => policy.ApplyDiscount(order));
            return bestDiscount;
        }
    }
}
