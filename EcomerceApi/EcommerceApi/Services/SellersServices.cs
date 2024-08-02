using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;
using EcommerceApi.Repositories;
using Npgsql.Internal.Postgres;

namespace EcommerceApi.Services;

public class SellersServices
{
    private readonly SellersRepository _sellersRepository;

    public SellersServices(SellersRepository sellersRepository)
    {
        _sellersRepository = sellersRepository;
    }


    public bool ValidReference(string reference)
    {
        return _sellersRepository.ValidReference(reference);
    }

}


