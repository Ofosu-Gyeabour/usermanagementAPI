#nullable disable 
namespace UserManagementAPI.Enums
{
    public enum OrderStatusEnum
    {
        INPUTTED = 1,
        APPROVED,
        DELIVERED_TO_CUSTOMER,
        COLLECTED_FROM_CUSTOMER,
        ADDED_TO_INVENTORY,
        ORDER_SHIPPED,
        ORDER_CANCELLED
    }

    public enum ItemStatusEnum
    {
        ORDERED = 1,
        APPROVED,
        SCANNED_TO_WAREHOUSE,
        SCANNED_FOR_SHIPPING
    }
}
