

const stripe = Stripe("pk_test_51REkOtGfGJcQyYM9IHpwrvBWQwXaIjMZYUj4SfZBqwJmsf9sdyAXdm5jYtY3FiQ4usiVLHxXZUVcUlqhVz56TV4u00a5q2WGyz");


    // Create a Checkout Session
    async function initialize() {
        //const fetchClientSecret = async () => {
        //    const response = await fetch("https://localhost:7144/api/stripeapi/Create", {
        //        method: "POST",
        //    });
        //    const { clientSecret } = await response.json();
        //    return clientSecret;
        //};

        //const checkout = await stripe.initEmbeddedCheckout({
        //    fetchClientSecret,
        //});

        //// Mount Checkout
        //checkout.mount('#checkout');


        if (event?.target?.getAttribute('data-buy') != '') {
            var productId = Number(event.target.getAttribute('data-buy'));

            if (isNaN(productId)) {
                throw 'Not a valid product';
            }

            var parentNode = event.target.parentNode;
            var email = parentNode.querySelector(`input[name='Email']`).value;


            const fetchClientSecret = async () => {
                const response = await fetch('https://localhost:7144/api/stripeapi', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        ProductId: productId,
                        Email: email
                    })
                });
                const { clientSecret } = await response.json();
                return clientSecret;
               
            };

            const checkout = await stripe.initEmbeddedCheckout({
                fetchClientSecret,
            });

            checkout.mount('#checkout');


                //then(async (response) => {
                //    console.log(response);
                //    if (response.status === 200) {
                //        const checkout = await stripe.initEmbeddedCheckout({
                //            fetchClientSecret,
                //        });

                //        checkout.mount('#checkout');
                //    }
                //});
        }
    }




document.querySelector("[data-buy]").addEventListener("click", async (event) => {
    initialize();    
});