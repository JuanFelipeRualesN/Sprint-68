using System;
using System.Configuration;
using System.Collections.Generic;
//using Stripe;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;
using Stripe;


namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To create or modify the products (set of item-products), for example: First grade progam, and the item-products (subject + level), example: English 1, Spanish 1, etc.
    /// </summary>
    public class ProductsCreatorManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The user's language code.</param>
        public ProductsCreatorManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To create a product. The product is a set of item-products together. For example, English 1 is an item product. The product is the 
        /// set of items together, such as "First grade program", and the item-products are "English 1", "Math 1", "Spanish 1" and so on.
        /// </summary>
        /// <param name="productModel">The product model to insert. </param>
        /// <param name="productItemsList">The product items to insert.</param>
        /// <param name="stripeApiKey">Stripe API Key if this is for commercial uses. If not, do not send it.</param>
        /// <param name="userSessionId">The user that is creating the record.</param>
        /// <returns>Returns the product response model with the transaction details.</returns>
        public ProductResponseModel CreateProduct(
            ProductModel productModel,
            List<ProductItemProductModel> productItemsList,
            string stripeApiKey,
            string userSessionId)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);
            var responseModel = new ProductResponseModel
            {
                TransactionValid = true,
                Messages = new List<string>(),
            };

            try
            {
                // Validate input
                if (productModel == null || string.IsNullOrWhiteSpace(productModel.Name))
                {
                    return new ProductResponseModel
                    {
                        TransactionValid = false,
                        Messages = new List<string>(),
                    };
                }

                string productId = Guid.NewGuid().ToString();

                if (!string.IsNullOrWhiteSpace(productModel.IsCommercial) && productModel.IsCommercial == "1")
                {
                    // Step 1: Create product in Stripe
                    StripeConfiguration.ApiKey = stripeApiKey;

                    var options = new ProductCreateOptions
                    {
                        Name = productModel.Name,
                        Active = true
                    };

                    var service = new Stripe.ProductService();
                    Stripe.Product productStripe = service.Create(options);

                    productId = productStripe.Id;
                }

                // Step 2: Store product in your own DB
                var productModelToCreate = new ProductModel
                {
                    Name = productModel.Name,
                    PlatformId = productId,
                    Available = "1",
                    IsCommercial = productModel.IsCommercial,
                    ModifiedBy = userSessionId,
                };

                var productDAO = new ProductDAO();
                var productModelCreated = productDAO.Create(_connectionString, productModelToCreate)[0];

                responseModel.Id = productModelCreated.Id;
                responseModel.Available = productModelCreated.Available;
                responseModel.CreatedAt = productModelCreated.CreatedAt;
                responseModel.Name = productModelCreated.Name;
                responseModel.PlatformId = productModelCreated.PlatformId;
                responseModel.UpdatedAt = productModelCreated.UpdatedAt;
                responseModel.IsCommercial = productModelCreated.IsCommercial;
                responseModel.ItemsIncluded = new List<ItemResponseModel>();

                // Step 3: Save included items
                for (int i = 0; i < productItemsList.Count; i++)
                {
                    productItemsList[i].ProductId = productModelCreated.Id;
                    productItemsList[i].ModifiedBy = userSessionId;
                }

                var productItemProductDAO = new ProductItemProductDAO();
                responseModel.ProductItemProductModelList = productItemProductDAO.Create(_connectionString, productItemsList);

                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_product"]));
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_product"]) + " " + ex.ToString());
                return responseModel;
            }
        }

        /// <summary>
        /// To update a product. The product is a set of item-products together. For example, English 1 is an item product. The product is the 
        /// set of items together, such as "First grade program", and the item-products are "English 1", "Math 1", "Spanish 1" and so on.
        /// </summary>
        /// <param name="productModel">The product model to update. </param>
        /// <param name="productItemsList">The product items to update.</param>
        /// <param name="stripeApiKey">Stripe API Key if this is for commercial uses. If not, do not send it.</param>
        /// <param name="userSessionId">The user that is updating the record.</param>
        /// <returns>Returns the product response model with the transaction details.</returns>
        public ProductResponseModel UpdateProduct(
            ProductModel productModel,
            List<ProductItemProductModel> productItemsList,
            string stripeApiKey,
            string userSessionId)
        {
            var productResponseModel = new ProductResponseModel
            {
                TransactionValid = true,
                Messages = new List<string>()
            };

            var messageManager = new MessageManager(_connectionStringRead, _langCode);

            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(productModel.Id) || string.IsNullOrWhiteSpace(productModel.Name))
                {
                    return new ProductResponseModel
                    {
                        TransactionValid = false,
                        Messages = new List<string>
                {
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_product"])
                }
                    };
                }

                // Step 1: Update product in DB
                var productDAO = new ProductDAO();
                var productModelUpdated = productDAO.Update(
                    _connectionString,
                    new ProductModel
                    {
                        Name = productModel.Name,
                        Available = productModel.Available,
                        IsCommercial = productModel.IsCommercial,
                        ModifiedBy = userSessionId
                    },
                    new ProductModel
                    {
                        Id = productModel.Id
                    }
                )[0];

                // Step 2: Update product in Stripe (if required)
                if (!string.IsNullOrWhiteSpace(productModel.PlatformId) && !string.IsNullOrWhiteSpace(productModel.IsCommercial) && productModel.IsCommercial == "1")
                {
                    var service = new Stripe.ProductService();
                    try
                    {
                        StripeConfiguration.ApiKey = stripeApiKey;

                        var options = new ProductUpdateOptions
                        {
                            Name = productModel.Name,
                            Active = productModel.Available == "1"
                        };

                        service.Update(productModel.PlatformId, options);
                    }
                    catch (Exception)
                    {
                        // Ignore Stripe update failures (optional: log for monitoring)
                    }

                    bool doesProductExist = true;
                    try
                    {
                        Stripe.Product stripeProduct = null;
                        stripeProduct = service.Get(productModel.PlatformId);
                    }
                    catch (StripeException ex) when (ex.StripeError.Code == "resource_missing")
                    {
                        doesProductExist = false;
                    }
                    catch (StripeException ex)
                    {
                        doesProductExist = false;
                    }
                    catch (Exception ex)
                    {
                        doesProductExist = false;
                    }

                    if (productModel.Available == "1" && !doesProductExist)
                    {
                        // Create it
                        var options = new ProductCreateOptions
                        {
                            Name = productModel.Name,
                            Active = true
                        };
                        Stripe.Product productStripe = service.Create(options);
                        productModelUpdated.PlatformId = productStripe.Id;

                        // Update the DAO. 
                        productModelUpdated = productDAO.Update(_connectionString,
                            new ProductModel
                            {
                                PlatformId = productStripe.Id,
                                ModifiedBy = userSessionId
                            },
                            new ProductModel
                            {
                                Id = productModel.Id
                            })[0];
                    }
                }

                // Step 3: Map to response
                productResponseModel.Id = productModel.Id;
                productResponseModel.Available = productModelUpdated.Available;
                productResponseModel.CreatedAt = productModelUpdated.CreatedAt;
                productResponseModel.Name = productModelUpdated.Name;
                productResponseModel.PlatformId = productModelUpdated.PlatformId;
                productResponseModel.UpdatedAt = productModelUpdated.UpdatedAt;
                productResponseModel.IsCommercial = productModelUpdated.IsCommercial;
                productResponseModel.ItemsIncluded = new List<ItemResponseModel>();

                // Step 4: Refresh items (delete old, insert new)
                var productItemDAO = new ProductItemProductDAO();

                productItemDAO.Delete(_connectionString, new ProductItemProductModel
                {
                    ProductId = productModel.Id
                });

                foreach (var item in productItemsList)
                {
                    item.ProductId = productModel.Id;
                    item.ModifiedBy = userSessionId;
                }

                productResponseModel.ProductItemProductModelList = productItemDAO.Create(_connectionString, productItemsList);

                // Step 5: Final response
                productResponseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_product"]));
                return productResponseModel;
            }
            catch (Exception ex)
            {
                productResponseModel.TransactionValid = false;
                productResponseModel.Messages.Add(
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_product"]) + " " + ex.ToString()
                );
                return productResponseModel;
            }
        }


        /// <summary>
        /// To create an item product. The item product is the subject + level together, so we can call the lessons related to this grade.
        /// </summary>
        /// <param name="itemToCreate">Item details to create.</param>
        /// <param name="userSessionId">The user who is creating the record.</param>
        /// <returns>Returns the response model details.</returns>
        public ItemProductResponseModel CreateItemProduct(ItemProductModel itemToCreate, string userSessionId)
        {
            try
            {
                var itemProductModel =
                new ItemProductDAO().Create
                    (_connectionString,
                    new ItemProductModel()
                    {
                        Available = itemToCreate.Available,
                        Name = itemToCreate.Name,
                        ModifiedBy = userSessionId
                    })[0];

                return new ItemProductResponseModel()
                {
                    TransactionValid = true,
                    ItemProductModel = itemProductModel,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_product"])
                    }
                };
            }
            catch (Exception ex)
            {
                return new ItemProductResponseModel()
                {
                    TransactionValid = false,
                    Messages = new List<string>()
                    {
                        ex.ToString()
                    }
                };
            }
        }

        /// <summary>
        /// Updates the item product.  The item product is the subject + level together, so we can call the lessons related to this grade.
        /// </summary>
        /// <param name="itemToUpdate">Item details to update.</param>
        /// <param name="userSessionId">The user who is updating the record.</param>
        /// <returns>Returns the response model details.</returns>
        public ItemProductResponseModel UpdateItemProduct(ItemProductModel itemToUpdate, string userSessionId)
        {
            try
            {
                // If there is no ID, return error.
                if (string.IsNullOrEmpty(itemToUpdate.Id))
                {
                    return new ItemProductResponseModel()
                    {
                        TransactionValid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_product"])
                        }
                    };
                }
                var itemProductResponseModel = new ItemProductResponseModel()
                {
                    TransactionValid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_product"])
                    },
                    ItemProductModel = new ItemProductDAO().Update(_connectionString,
                    new ItemProductModel()
                    {
                        Name = itemToUpdate.Name,
                        Available = itemToUpdate.Available,
                        ModifiedBy = userSessionId
                    }, new ItemProductModel()
                    {
                        Id = itemToUpdate.Id,
                    })[0]
                };

                return itemProductResponseModel;
            }
            catch (Exception ex)
            {
                return new ItemProductResponseModel()
                {
                    TransactionValid = false,
                    Messages = new List<string>()
                    {
                        ex.ToString()
                    }
                };
            }
        }
    }
}
