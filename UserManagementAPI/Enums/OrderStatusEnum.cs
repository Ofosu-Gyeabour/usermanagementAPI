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
        ORDER_CANCELLED,
        CONSOLIDATOR_POSTED
    }

    public enum ConsolOrderStatusEnum
    {
        PENDING = 1,
        PROCESSED
    }
}
