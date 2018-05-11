﻿using Facturacion.HtmlHelpers;
using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion.ModelView
{
    public class ArticuloViewModels
    {
        public IEnumerable<Articulo> Articulos { get; set; }
        public PagingInfoViewModels PagingInfo { get; set; }

    }
}