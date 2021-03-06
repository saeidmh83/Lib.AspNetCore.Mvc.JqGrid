﻿using System;
using System.Linq;
using System.Text;
using Lib.AspNetCore.Mvc.JqGrid.Helper.Constants;
using Lib.AspNetCore.Mvc.JqGrid.Infrastructure.Constants;
using Lib.AspNetCore.Mvc.JqGrid.Infrastructure.Options;

namespace Lib.AspNetCore.Mvc.JqGrid.Helper.InternalHelpers
{
    internal static class JqGridSubgridJavaScriptRenderingHelper
    {
        #region Extension Methods
        internal static StringBuilder AppendSubgrid(this StringBuilder javaScriptBuilder, JqGridOptions options)
        {
            if (options.SubgridEnabled && (options.SubgridOptions != null))
            {
                javaScriptBuilder.AppendCoreSubgridOptions(options)
                    .AppendSubgridAsGridRowExpanded(options);
            }
            else if (options.SubgridEnabled && !String.IsNullOrWhiteSpace(options.SubgridUrl) && (options.SubgridModel != null))
            {
                javaScriptBuilder.AppendCoreSubgridOptions(options)
                    .AppendJavaScriptObjectFunctionField(JqGridOptionsNames.SUBGRID_ROW_EXPANDED, options.SubGridRowExpanded)
                    .AppendJavaScriptObjectStringField(JqGridOptionsNames.SUBGRID_ULR, options.SubgridUrl)
                    .AppendJavaScriptArrayFieldOpening(JqGridOptionsNames.SUBGRID_MODEL)
                    .AppendJavaScriptObjectOpening()
                    .AppendJavaScriptObjectStringArrayField(JqGridOptionsNames.SubgridModel.NAMES, options.SubgridModel.ColumnsModels.Select(c => c.Name))
                    .AppendJavaScriptObjectIntegerArrayField(JqGridOptionsNames.SubgridModel.WIDTHS, options.SubgridModel.ColumnsModels.Select(c => c.Width))
                    .AppendJavaScriptObjectEnumArrayField(JqGridOptionsNames.SubgridModel.ALIGNMENTS, options.SubgridModel.ColumnsModels.Select(c => c.Alignment))
                    .AppendJavaScriptObjectStringArrayField(JqGridOptionsNames.SubgridModel.MAPPINGS, options.SubgridModel.ColumnsModels.Select(c => c.Mapping))
                    .AppendJavaScriptObjectStringArrayField(JqGridOptionsNames.SubgridModel.PARAMETERS, options.SubgridModel.Parameters)
                    .AppendJavaScriptObjectFieldClosing()
                    .AppendJavaScriptArrayFieldClosing();
            }

            return javaScriptBuilder;
        }
        #endregion

        #region Private Methods
        private static StringBuilder AppendCoreSubgridOptions(this StringBuilder javaScriptBuilder, JqGridOptions options)
        {
            javaScriptBuilder.AppendJavaScriptObjectBooleanField(JqGridOptionsNames.SUBGRID_ENABLED, true)
                    .AppendJavaScriptObjectIntegerField(JqGridOptionsNames.SUBGRID_WIDTH, options.SubgridColumnWidth, JqGridOptionsDefaults.SubgridColumnWidth)
                    .AppendJavaScriptObjectFunctionField(JqGridOptionsNames.SUBGRID_BEFORE_EXPAND, options.SubGridBeforeExpand)
                    .AppendJavaScriptObjectFunctionField(JqGridOptionsNames.SUBGRID_ROW_COLAPSED, options.SubGridRowColapsed);

            return javaScriptBuilder;
        }

        private static StringBuilder AppendSubgridAsGridRowExpanded(this StringBuilder javaScriptBuilder, JqGridOptions options)
        {
            javaScriptBuilder.AppendFormat("{0}: function (subgridId, rowId) {{", JqGridOptionsNames.SUBGRID_ROW_EXPANDED)
                .Append("var $subgridContainer = jQuery('#' + subgridId);")
                .Append("var $subgridTable = jQuery('<table></table').attr('id', subgridId + '_t');")
                .Append("$subgridContainer.append($subgridTable);");

            if (options.SubgridOptions.Pager)
            {
                javaScriptBuilder.Append("var $subgridPager = jQuery('<div></div>').attr('id', subgridId + '_p');")
                    .Append("$subgridContainer.append($subgridPager);");
            }

            javaScriptBuilder.Append("$subgridTable")
                .AppendJqGridScript(options.SubgridOptions, true)
                .Append("},");

            return javaScriptBuilder;
        }
        #endregion
    }
}
