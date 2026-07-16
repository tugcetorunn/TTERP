using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Supplies;

namespace TTERP.Application.Helpers
{
    public static class WorkflowActionHelper
    {
        public static WorkflowActionPermissionsDTO CreateActions(
            int workflowType,
            string? statusShortCode)
        {
            var status = statusShortCode?.Trim().ToLowerInvariant();

            return workflowType switch
            {
                1 => CreateSupplyActions(status),
                2 => CreateProductionActions(status),
                3 => CreateSalesActions(status),
                _ => new WorkflowActionPermissionsDTO()
            };
        }

        private static WorkflowActionPermissionsDTO CreateSupplyActions(string? status)
        {
            return status switch
            {
                "planned" => new WorkflowActionPermissionsDTO
                {
                    CanEdit = true,
                    CanDelete = true,
                    CanAddItem = true,
                    CanCancel = true,
                    CanPrint = true
                },
                "ordered" => new WorkflowActionPermissionsDTO
                {
                    CanEdit = true,
                    CanDelete = false,
                    CanAddItem = true,
                    CanCancel = true,
                    CanPrint = true
                },
                "in-transit" => new WorkflowActionPermissionsDTO
                {
                    CanEdit = false,
                    CanDelete = false,
                    CanAddItem = false,
                    CanCancel = true,
                    CanPrint = true
                },
                _ => new WorkflowActionPermissionsDTO
                {
                    CanPrint = true
                }
            };
        }

        private static WorkflowActionPermissionsDTO CreateProductionActions(string? status)
        {
            return status switch
            {
                "planned" => new WorkflowActionPermissionsDTO
                {
                    CanEdit = true,
                    CanDelete = true,
                    CanAddItem = true,
                    CanCancel = true,
                    CanPrint = true
                },
                "started" => new WorkflowActionPermissionsDTO
                {
                    CanEdit = false,
                    CanDelete = false,
                    CanAddItem = false,
                    CanCancel = true,
                    CanPrint = true
                },
                "paused" => new WorkflowActionPermissionsDTO
                {
                    CanEdit = false,
                    CanDelete = false,
                    CanAddItem = false,
                    CanCancel = true,
                    CanPrint = true
                },
                _ => new WorkflowActionPermissionsDTO
                {
                    CanPrint = true
                }
            };
        }

        private static WorkflowActionPermissionsDTO CreateSalesActions(string? status)
        {
            return new WorkflowActionPermissionsDTO
            {
                CanPrint = true
            };
        }
    }
}
