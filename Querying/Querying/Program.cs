







/*namespace QueryNamespace {

    public class Order {

    }

    public class Product {

    }

    public class Querying {

        public Order getOrder() {
            List<Product> cartItems;*/

            var order = new Order();
            var productTasks = new List<Task<Product>>();
            var priceTasks = new List<Task<decimal>>();

            foreach (var cartItem in cartItems) {
                var productTask = dbContext.Products.SingleOrDefaultAsync(product => product.Number == cartItem.Number);
                productTasks.Add(productTask);
            }

            await Task.WhenAll(productTasks);

            foreach(var cartItem in cartItems) {
                var matchingProduct = productTasks[cartItems.IndexOf(cartItem)].Result;
                var priceTask = priceQuerier.GetCustomerPriceAsync(customerNumber, cartItem.ProductNumber);
                priceTasks.Add(priceTask);

                order.Lines.Add(new OrderLine {
                    ProductId = matchingProduct.Id,
                    ProductNumber = cartItem.ProductNumber,
                    Quantity = cartItem.Quantity
                });
            }

            await Task.WhenAll(priceTasks);

            for(int i = 0; i < cartItems.Count; i++) {
                order.Lines[i].UnitPrice = priceTasks[i].Result;
            }

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            
            return order;
        /*}

        public static async void Main() {


        }
    }
}*/