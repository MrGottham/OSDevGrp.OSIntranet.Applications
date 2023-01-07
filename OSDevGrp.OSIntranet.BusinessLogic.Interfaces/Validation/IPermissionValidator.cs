namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IPermissionValidator
    {
        IValidator HasNecessaryPermission(bool necessaryPermissionGranted);
    }
}