using Authoritiy.IServices.BaseServices;
using Authority.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices
{
   public  interface IProductServices : IBaseEntity<Product>
    {

        Task<bool> AddProduct(Product product); //增加商品

        Task<bool> DelProduct(Product product); //删除商品

        Task<Product> EditProduct(Product product); //编辑商品 

        Task<bool> Modfiy(Product product); //保存

        Task<List<Product>> QueryProductList(); //查询所有的商品

        Task<List<Product>> QueryListInforName(string ProductName);//根据商品名称查询

        Task<Product> QueryListInforquantity(int Id);//查询库存

    }
}
