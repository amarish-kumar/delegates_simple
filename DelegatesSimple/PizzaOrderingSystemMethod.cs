using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesSimple {
    class PizzaOrderingSystemMethod {
        private readonly DiscountPolicyMethods _discountPolicyMethods;
        public PizzaOrderingSystemMethod() {
            _discountPolicyMethods = new DiscountPolicyMethods();
        }
        public decimal ComputePrice(PizzaOrder order) {
            decimal total = order.Pizzas.Sum(p => p.Price);

            decimal[] discounts = new[] {
                _discountPolicyMethods.BuyOneGetOneFree(order),
                _discountPolicyMethods.FivePercentOffMoreThanFiftyDollars(order),
                _discountPolicyMethods.FiveDollarsOffStuffedCrust(order),
            };

            decimal bestDiscount = discounts.Max(discount => discount);
            total = total - bestDiscount;
            return total;
        }
    }

    public class DiscountPolicyMethods {
        public decimal BuyOneGetOneFree(PizzaOrder order) {
            var pizzas = order.Pizzas;
            if (pizzas.Count < 2) {
                return 0m;
            }
            return pizzas.Min(p => p.Price);
        }
        public decimal FivePercentOffMoreThanFiftyDollars(PizzaOrder order) {
            decimal nonDiscounted = order.Pizzas.Sum(p => p.Price);
            return nonDiscounted >= 50 ? nonDiscounted * 0.05m : 0M;

        }
        public decimal FiveDollarsOffStuffedCrust(PizzaOrder order) {
            return order.Pizzas.Sum(p => p.Crust == Crust.Stuffed ? 5m : 0m);
        }
    }
}
