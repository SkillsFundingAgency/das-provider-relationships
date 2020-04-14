using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues.Operations;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Operations;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    public class OperationsHelper
    {
        private readonly OperationRouteValue _routeValue;
        private readonly TempDataDictionary _tempData;
        private readonly UrlHelper _urlHelper;
        public List<OperationViewModel> Changes { get; }
        public Operation CurrentOperation { get; }

        public OperationsHelper(OperationRouteValue routeValue, TempDataDictionary tempData, UrlHelper urlHelper)
        {
            _routeValue = routeValue;
            _tempData = tempData;
            _urlHelper = urlHelper;

            Changes = tempData.Get<List<OperationViewModel>>() ?? new List<OperationViewModel>();

            CurrentOperation = (Operation)Enum.Parse(typeof(Operation), _routeValue.OperationId.ToString());

            if (CurrentOperation.Equals(Operation.NotSet))
            {
                _tempData.Clear();
                CurrentOperation = AllowedOperations().First();
            }
        }

        public void Set(UpdateOperationViewModel model)
        {
            model.Operation = CurrentOperation;
            model.IsEnabled = IsEnabled(model) ?? false;
            model.Operations = Changes;
            model.BackLink = GetBackLink();
            _tempData.Set(Changes);
        }

        public void Update(UpdateOperationViewModel model)
        {   
            Changes.First(o => o.Value.Equals(CurrentOperation)).IsEnabled = _routeValue.IsEnabled;
            Set(model);         
        }

        public void Confirm(ConfirmOperationViewModel model)
        {
            model.Operations = Changes;

            var previousOperation = AllowedOperations().Last();
            if (previousOperation == Operation.NotSet)
            {
                model.BackLink = _urlHelper.Action("Index", "AccountProviders");
            }
            else
            {
                model.BackLink = _urlHelper.Action("Set", "Operations",
                    new OperationRouteValue {
                        AccountProviderId = _routeValue.AccountProviderId,
                        AccountLegalEntityId = _routeValue.AccountLegalEntityId,
                        OperationId = (short)previousOperation
                    });
            }

            _tempData.Set(Changes);
        }

        public bool IsValid()
        {
            return CurrentOperation != Operation.NotSet && _routeValue.IsEnabled.HasValue;
        }

        public Operation Next()
        {
            if (!AllowedOperations().IsLast(CurrentOperation) && !_routeValue.IsEditMode)
            {
                return AllowedOperations().Next(CurrentOperation);
            }

            return Operation.NotSet;
        }

        private bool? IsEnabled(UpdateOperationViewModel model)
        {
            if (Changes.Any(o => o.Value.Equals(CurrentOperation)))
            {
                return Changes.First(o => o.Value.Equals(CurrentOperation)).IsEnabled;
            }
            else
            {
                var isEnabled = model.Operations.FirstOrDefault(o => o.Value.Equals(CurrentOperation))?.IsEnabled;
                Changes.Add(new OperationViewModel { Value = CurrentOperation, IsEnabled = isEnabled });
                return isEnabled;
            }
        }

        private string GetBackLink()
        {
            if (_routeValue.IsEditMode)
            {
                return _urlHelper.Action("Confirm", "Operations",
                       new OperationRouteValue {
                           AccountProviderId = _routeValue.AccountProviderId,
                           AccountLegalEntityId = _routeValue.AccountLegalEntityId
                       });
            }
            if (_routeValue.OperationId < 0 || CurrentOperation == AllowedOperations().First())
            {
                return _urlHelper.Action("Index", "AccountProviders");
            }
            
            return _urlHelper.Action("Set", "Operations",
                new OperationRouteValue {
                    AccountProviderId = _routeValue.AccountProviderId,
                    AccountLegalEntityId = _routeValue.AccountLegalEntityId,
                    OperationId = (short)AllowedOperations().Previous(CurrentOperation)
                });
        }

        private List<Operation> AllowedOperations()
        {
            var allowedList = new List<Operation>();

            foreach (short value in Enum.GetValues(typeof(Operation)))
            {
                if (value != (short)Operation.NotSet)
                {
                    allowedList.Add((Operation)Enum.Parse(typeof(Operation), value.ToString()));
                }
            }

            return allowedList;
        }
    }
}