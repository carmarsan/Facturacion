using Facturacion.HtmlHelpers;
using Facturacion.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfoViewModels pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString PageLinks2(this HtmlHelper html, PagingInfoViewModels pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder resultFinal = new StringBuilder();

            TagBuilder divWell = new TagBuilder("div");
            divWell.AddCssClass("well well-sm");

            TagBuilder tagFirst = new TagBuilder("a");
            if (pagingInfo.CurrentPage != 1)
            {
                tagFirst.MergeAttribute("href", pageUrl(1));
                tagFirst.AddCssClass("glyphicon glyphicon-fast-backward");
            }
            else
                tagFirst.AddCssClass("glyphicon glyphicon-fast-backward text-muted");
            tagFirst.MergeAttribute("style", "margin-right: 10px;");
            //tagFirst.AddCssClass("btn btn-primary btn-md");
            //tagFirst.SetInnerText("<<");
            //if (pagingInfo.CurrentPage == 1)
            //    tagFirst.AddCssClass("disabled");

            result.Append(tagFirst);


            // ******************************************************
            // Botón anterios <<

            TagBuilder tagPrevious = new TagBuilder("a");
            tagPrevious.AddCssClass("glyphicon glyphicon-backward");
            if (pagingInfo.CurrentPage != 1)
                tagPrevious.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
            else
                tagPrevious.AddCssClass("text-muted");
            tagPrevious.MergeAttribute("style", "margin-right: 10px;");
            //tagPrevious.AddCssClass("btn btn-primary btn-md");
            //tagPrevious.SetInnerText("<");
            //if (pagingInfo.CurrentPage == 1)
            //    tagPrevious.AddCssClass("disabled");
            result.Append(tagPrevious);


            // Texto de "Pag:"
            TagBuilder pagina1 = new TagBuilder("label");
            pagina1.MergeAttribute("style", "margin-right: 10px");
            pagina1.InnerHtml = " Pág: ";

            result.Append(pagina1);

            // ***************************************
            // DropDown con los números de las páginas

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("dropdown");
            //div.MergeAttribute("style", "display:inline-block; margin: 0px 10px");
            div.MergeAttribute("style", "display:inline-block;");

            TagBuilder button = new TagBuilder("button");
            button.AddCssClass("btn btn-default  dropdown-toggle");
            button.Attributes.Add("data-toggle", "dropdown");
            button.MergeAttribute("style", "min-width: 75px;");
            button.InnerHtml = pagingInfo.CurrentPage.ToString();

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("caret");
            span.MergeAttribute("style", "margin-lef: 10px");

            button.InnerHtml += span;

            div.InnerHtml = button.ToString();

            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("dropdown-menu");
            ul.MergeAttribute("style", "min-width: 75px;");


            var items = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", pageUrl(i));

                a.InnerHtml = i.ToString();
                li.InnerHtml = a.ToString();

                items.Append(li);

            }

            ul.InnerHtml = items.ToString();
            div.InnerHtml += ul;

            result.Append(div);

            // INPUT con Página X de XX
            // Texto en medio
            TagBuilder pagina = new TagBuilder("label");
            pagina.MergeAttribute("style", "margin: 0px 10px");
            pagina.InnerHtml = string.Format(" de {0} ", pagingInfo.TotalPages.ToString());

            result.Append(pagina);


            TagBuilder tagNext = new TagBuilder("a");
            tagNext.AddCssClass("glyphicon glyphicon-forward");
            if (pagingInfo.CurrentPage != pagingInfo.TotalPages)
                tagNext.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
            else
                tagNext.AddCssClass("text-muted");
            tagNext.MergeAttribute("style", "margin-right: 10px;");

            result.Append(tagNext);

            TagBuilder tagLast = new TagBuilder("a");
            if (pagingInfo.CurrentPage != pagingInfo.TotalPages)
                tagLast.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
            else
                tagLast.AddCssClass("text-muted");
            tagLast.AddCssClass("glyphicon glyphicon-fast-forward");
            tagLast.MergeAttribute("style", "margin-right: 10px;");

            result.Append(tagLast);


            // Vamos a poner un drowdown para que se elija el número de registros a visualizar
            //  10, 20, 30, 40
            // El primer número lo vamos a sacar del web.config


            //result.Append(GetDropDownNumberOfPages());

            divWell.InnerHtml = result.ToString();

            resultFinal.Append(divWell.ToString());

            //List<SelectListItem> list = new List<SelectListItem>();

            //for (int i = 0; i < pagingInfo.TotalPages; i++)
            //{
            //    SelectListItem item = new SelectListItem();
            //    item.Value = i.ToString();
            //    item.Text = i.ToString();
            //    item.Selected = i == pagingInfo.CurrentPage;
            //    list.Add(item);
            //}


            //TagBuilder tagSelect = new TagBuilder("select");
            //tagSelect.MergeAttribute("id", "idSelect");
            //tagSelect.MergeAttribute("name", "idSelect");
            //tagSelect.AddCssClass("dropdown");

            //var options = new StringBuilder();
            //for (int i = 1; i <= pagingInfo.TotalPages; i++)
            //{
            //    var option = new TagBuilder("option");
            //    option.InnerHtml = i.ToString();
            //    option.MergeAttribute("href", pageUrl(i));
            //    if (i == pagingInfo.CurrentPage)
            //        option.AddCssClass("selected");
            //    options.Append(option);
            //}

            //tagSelect.InnerHtml = options.ToString();


            //for (int i = 1; i <= pagingInfo.TotalPages; i++)
            //{
            //    TagBuilder tag = new TagBuilder("a");
            //    tag.MergeAttribute("href", pageUrl(i));
            //    tag.InnerHtml = i.ToString();
            //    if (i == pagingInfo.CurrentPage)
            //    {
            //        tag.AddCssClass("selected");
            //        tag.AddCssClass("btn-primary");
            //    }
            //    tag.AddCssClass("btn btn-default");
            //    result.Append(tag.ToString());
            //}
            return MvcHtmlString.Create(resultFinal.ToString());
        }


        //private static TagBuilder GetDropDownNumberOfPages(PagingInfoViewModels pagingInfo)
        //{
        //    int primerNumero = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PAGE_SIZE"]);

        //    TagBuilder div = new TagBuilder("div");
        //    div.AddCssClass("dropdown");
        //    //div.MergeAttribute("style", "display:inline-block; margin: 0px 10px");
        //    div.MergeAttribute("style", "display:inline-block;");

        //    TagBuilder button = new TagBuilder("button");
        //    button.AddCssClass("btn btn-default  dropdown-toggle");
        //    button.Attributes.Add("data-toggle", "dropdown");
        //    button.MergeAttribute("style", "min-width: 75px;");
        //    button.InnerHtml = primerNumero.ToString();

        //    TagBuilder span = new TagBuilder("span");
        //    span.AddCssClass("caret");
        //    span.MergeAttribute("style", "margin-lef: 10px");

        //    button.InnerHtml += span;

        //    div.InnerHtml = button.ToString();

        //    TagBuilder ul = new TagBuilder("ul");
        //    ul.AddCssClass("dropdown-menu");
        //    ul.MergeAttribute("style", "min-width: 75px;");


        //    var items = new StringBuilder();
        //    for (int i = 1; i <= pagingInfo.ItemsPerPage; i++)
        //    {
        //        TagBuilder li = new TagBuilder("li");
        //        TagBuilder a = new TagBuilder("a");
        //        //a.MergeAttribute("href", pageUrl(i));

        //        a.InnerHtml = i.ToString();
        //        li.InnerHtml = a.ToString();

        //        items.Append(li);

        //    }

        //    ul.InnerHtml = items.ToString();
        //    div.InnerHtml += ul;

        //    return div;
        //}
    }
}