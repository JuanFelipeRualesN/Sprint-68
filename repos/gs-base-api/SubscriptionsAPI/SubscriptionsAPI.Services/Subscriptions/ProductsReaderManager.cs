using System.Collections.Generic;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using System.Runtime.InteropServices;
using System;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To read the products (set of item-products), for example: First grade progam, and the item-products (subject + level), example: English 1, Spanish 1, etc.
    /// </summary>
    public class ProductsReaderManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connections'string (read)</param>
        /// <param name="langCode">The user's language code.</param>
        public ProductsReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the product details by filtering by product id.
        /// </summary>
        /// <param name="productId">The product's id to filter.</param>
        /// <param name="onlyAvailable">To call only the avialbale items.</param>
        /// <param name="callItems">To call the item-products (example: Science 1, English 1, etc.)</param>
        /// <returns>Returns the products.</returns>
        public ProductResponseModel GetProductResponseModelListByProductId(string productId, bool onlyAvailable, bool callItems)
        {
            // First, we find the product details 
            var productToFind = new ProductModel
            {
                Id = productId
            };
            if (onlyAvailable)
                productToFind.Available = "1";

            var productDAO = new ProductDAO();
            var productsFound = productDAO.Read(_connectionStringRead, productToFind, true, true);

            if (productsFound.Count == 0)
                return null;

            // Now return the response 
            var productResponseModel = new ProductResponseModel
            {
                Id = productsFound[0].Id,
                Available = productsFound[0].Available,
                CreatedAt = productsFound[0].CreatedAt,
                UpdatedAt = productsFound[0].UpdatedAt,
                PlatformId = productsFound[0].PlatformId,
                IsCommercial = productsFound[0].IsCommercial,
                Name = productsFound[0].Name,
            };

            if (callItems) // Call the items that are included if the you want them 
                productResponseModel.ItemsIncluded = GetItemsResponseListByProductId(productsFound[0].Id, onlyAvailable);

            return productResponseModel;
        }

        /// <summary>
        /// Returns the item products by filtering by product-item relation model.
        /// </summary>
        /// <param name="productItemProductModelList">The product-item-relation model to filter.</param>
        /// <returns>The itm products filtered.</returns>
        public List<ItemResponseModel> GetItemResponseByProductListId(List<ProductItemProductModel> productItemProductModelList)
        {

            // Read the product-item relation.
            var productItemsProductFoundList = new ProductItemProductDAO().Read(_connectionStringRead, productItemProductModelList);
            if (productItemsProductFoundList.Count == 0)
            {
                return new List<ItemResponseModel>();
            }

            // Find the items.
            var itemProductToFindList = new List<ItemProductModel>();
            foreach (var productItemProduct in productItemProductModelList)
            {
                itemProductToFindList.Add(new ItemProductModel()
                {
                    Id = productItemProduct.ItemId,
                    Available = "1",
                });
            }
            var itemProductFoundList = new ItemProductDAO().Read(_connectionStringRead, itemProductToFindList);
            if (itemProductFoundList.Count == 0)
            {
                return new List<ItemResponseModel>();
            }

            // Now prepare the item response to return. 
            var itemResponseModelList = new List<ItemResponseModel>();
            foreach (var itemProduct in itemProductFoundList)
            {
                itemResponseModelList.Add(new ItemResponseModel()
                {
                    Id = itemProduct.Id,
                    Available = itemProduct.Available,
                    CreatedAt = itemProduct.CreatedAt,
                    ModifiedBy = itemProduct.ModifiedBy,
                    Name = itemProduct.Name,
                    UpdatedAt = itemProduct.UpdatedAt,
                    ProductId = productItemsProductFoundList.Find(x => x.ItemId == itemProduct.Id)?.ProductId
                });
            }

            return itemResponseModelList;
        }

        /// <summary>
        /// To get the items of a product by filtering by product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="onlyAvailable">true to call only the available items, else false.</param>
        /// <returns>Return the list of items of a product.</returns>
        public List<ItemResponseModel> GetItemsResponseListByProductId(string productId, bool onlyAvailable)
        {
            var itemResponseModelList = new List<ItemResponseModel>();

            var productItemProductModel = new ProductItemProductModel
            {
                ProductId = productId
            };

            var productItemProductDAO = new ProductItemProductDAO();
            var productItemsProductFoundList = productItemProductDAO.Read(_connectionStringRead, productItemProductModel, false, false);

            // Now find the Product Items Model 
            var itemProductToFindList = new List<ItemProductModel>();

            for (int i = 0; i < productItemsProductFoundList.Count; i++)
            {
                var itemProductModel = new ItemProductModel
                {
                    Id = productItemsProductFoundList[i].ItemId
                };
                if (onlyAvailable)
                    itemProductModel.Available = "1";

                itemProductToFindList.Add(itemProductModel);
            }
            var itemProductDAO = new ItemProductDAO();
            var itemProductFoundList = itemProductDAO.Read(_connectionStringRead, itemProductToFindList);

            // Now connect everything 
            for (int i = 0; i < itemProductFoundList.Count; i++)
            {
                var itemResponseModel = new ItemResponseModel
                {
                    Id = itemProductFoundList[i].Id,
                    Available = itemProductFoundList[i].Available,
                    CreatedAt = itemProductFoundList[i].CreatedAt,
                    ModifiedBy = itemProductFoundList[i].ModifiedBy,
                    Name = itemProductFoundList[i].Name,
                    UpdatedAt = itemProductFoundList[i].UpdatedAt,
                    ProductId = productId
                };
                itemResponseModelList.Add(itemResponseModel);
            }
            return itemResponseModelList;
        }

       /// <summary>
       /// To get the products. The products are a set of item products. For example: "First grade" as product, and "Enlish 1", "Spanish 1", etc as item products.
       /// </summary>
       /// <param name="onlyAvailables">To call only the available products.</param>
       /// <param name="includedItemsRequired">To include the items (true), otherwise (false).</param>
       /// <param name="isCommercial">To read only the commercial plans.</param>
       /// <returns>Returns the products with item product's details.</returns>
        public List<ProductResponseModel> GetProducts(bool onlyAvailables, bool includedItemsRequired, bool isCommercial = false)
        {
            // Retrieve the product models
            var productModels = new ProductDAO().Read(_connectionStringRead, new ProductModel { Available = onlyAvailables ? "1" : null, IsCommercial = isCommercial ? "1": "0" }, false, false);

            // Prepate the products to call the included items if applies 
            var productItemProducts = new List<ProductItemProductModel>();
            for (int i = 0; i < productModels.Count; i++)
            {
                productItemProducts.Add(new ProductItemProductModel()
                {
                    ProductId = productModels[i].Id,
                    Available = onlyAvailables ? "1" : null
                });
            }

            if (!includedItemsRequired)
            {

                // Now prepare the results 
                var productResponseModelResult = new List<ProductResponseModel>();
                for (int i = 0; i < productModels.Count; i++)
                {
                    productResponseModelResult.Add(new ProductResponseModel
                    {
                        Available = productModels[i].Available,
                        Name = productModels[i].Name,
                        Id = productModels[i].Id,
                        IsCommercial = productModels[i].IsCommercial,
                        PlatformId = productModels[i].PlatformId,
                        CreatedAt = productModels[i].CreatedAt,
                        UpdatedAt = productModels[i].UpdatedAt
                    });
                }

                // Order by name
                if (productResponseModelResult.Count > 0)
                {
                    productResponseModelResult.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));
                }

                return productResponseModelResult;
            }
            else
            {
                // Attach the included items 
                var itemResponseModelList = GetItemResponseByProductListId(productItemProducts);
                // Now filter and add 
                // Now prepare the results 
                var productResponseModelResult = new List<ProductResponseModel>();
                for (int i = 0; i < productModels.Count; i++)
                {
                    productResponseModelResult.Add(new ProductResponseModel
                    {
                        Available = productModels[i].Available,
                        Name = productModels[i].Name,
                        Id = productModels[i].Id,
                        PlatformId = productModels[i].PlatformId,
                        IsCommercial = productModels[i].IsCommercial,
                        CreatedAt = productModels[i].CreatedAt,
                        UpdatedAt = productModels[i].UpdatedAt,
                        ItemsIncluded = itemResponseModelList.FindAll(x => x.ProductId == productModels[i].Id)
                    });
                }
                if (productResponseModelResult.Count > 0)
                {
                    productResponseModelResult.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));
                }
                return productResponseModelResult;
            }
        }

        /// <summary>
        /// Returns the item products. The item products are the subject + level code. Example: English 1.
        /// </summary>
        /// <param name="itemProductModelToFilter">The item products to filter.</param>
        /// <returns>Returns the item products.</returns>
        public List<ItemProductModel> GetItemProducts(ItemProductModel itemProductModelToFilter)
        {
            return new ItemProductDAO().Read(_connectionStringRead, itemProductModelToFilter, false, false);
        }
    }
}
