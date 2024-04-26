#nullable disable
using System.Configuration;
using System.Runtime.InteropServices;
using UserManagementAPI.Enums;
using UserManagementAPI.Models;
using UserManagementAPI.utils;
namespace UserManagementAPI.POCOs
{

    public class clsShippingContainer
    {
        public clsShippingContainer()
        {
            
        }

        public int? containerTypeId { get; set; }
        public string nameOfcontainer { get; set; }
        public DateTime? dateCreated { get; set; }
        public int? receivingAgentId { get; set; }
        public string? receivingAgent { get; set; }

        public int? consignorAgentId { get; set; }
        public string? consignorAgent { get; set; }
        public string? bookingReference { get; set; }
        public decimal? freight { get; set; }
        public decimal? haulage { get; set; }
        public decimal? volumeOfcontainer { get; set; }
        public int? sealNumber { get; set; }
        public int? bookedWithShippingLineId { get; set; }
        public string? bookedWithShippingLine { get; set; }
        public int? quantity { get; set; }
        public string? containerRef { get; set; }
        public string containerCode { get; set; }
        public int? scheduleID { get; set; }

        public string? vesselName { get; set; } = string.Empty;
        public string? ets { get; set; } = string.Empty;
        public string? eta { get; set; } = string.Empty;
        public string? loadingPort { get; set; } = string.Empty;
        public string? dischargePort { get; set; } = string.Empty;
        public string? island { get; set; } = string.Empty;
        public string? shippingline { get; set; } = string.Empty;

        public async Task<bool> AddContainerAsync(List<GenericLookup> containerStats, List<GenericLookup> documentList)
        {
            //TODO: adds container to the container data store
            bool blnStatus = false;
            Helper helper = new Helper();

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    var transaction = await config.Database.BeginTransactionAsync();

                    try
                    {
                        TLoadContainer loadContainer = new TLoadContainer() { 
                            ContainerTypeId = containerTypeId,
                            ContainerName = nameOfcontainer,
                            DteCreated = dateCreated,
                            ReceivingAgentId = receivingAgentId,
                            ConsignorAgentId = consignorAgentId,
                            BookingRef = bookingReference,
                            Freight = freight,
                            Haulage = haulage,
                            ContainerVolume = await getContainerVolumeAsync(),
                            SealNo = sealNumber,
                            BookedwithShippingLineId = bookedWithShippingLineId,
                            Qty = quantity,
                            ContainerStatusId = (int) ContainerLoadingStatus.ContainerLoadingStatusEnum.NOT_COMPLETED,
                            ContainerCode = containerCode,
                            ScheduleId = scheduleID
                        };

                        await config.AddAsync(loadContainer);
                        await config.SaveChangesAsync();

                        //tcontainerstatisticslookup table
                        int containerId = loadContainer.Id;

                        foreach(var cs in containerStats)
                        {
                            TLoadContainerStatistic loadContainerStatistic = new TLoadContainerStatistic() { 
                                ContainerId = containerId,
                                StatId = cs.id,
                                StatValue = 0m
                            };
                            
                            await config.AddAsync(loadContainerStatistic);
                            await config.SaveChangesAsync();
                        }

                        foreach (var doc in documentList)
                        {
                            TLoadContainerDocValue cdValue = new TLoadContainerDocValue()
                            {
                                ContainerId = containerId,
                                TcontainerDocLookupId = doc.id
                            };

                            switch (doc.id)
                            {
                                case 1:
                                    cdValue.TcontainerDocValue = receivingAgent;
                                    break;
                                case 2:
                                    cdValue.TcontainerDocValue = await helper.getAgentEmailAsync((int)receivingAgentId);
                                    break;
                                case 3:
                                    cdValue.TcontainerDocValue = await helper.getVesselReferenceAsync(containerId);
                                    break;
                                case 4:
                                    cdValue.TcontainerDocValue = vesselName;
                                    break;
                                case 7:
                                    cdValue.TcontainerDocValue = ets;
                                    break;
                                case 8:
                                    cdValue.TcontainerDocValue = eta;
                                    break;
                                case 9:
                                    cdValue.TcontainerDocValue = loadingPort;
                                    break;
                                case 10:
                                    cdValue.TcontainerDocValue = dischargePort;
                                    break;
                                case 11:
                                    cdValue.TcontainerDocValue = island;
                                    break;
                                case 12:
                                    cdValue.TcontainerDocValue = nameOfcontainer;
                                    break;
                                case 13:
                                    cdValue.TcontainerDocValue = sealNumber.ToString();
                                    break;
                                case 14:
                                    cdValue.TcontainerDocValue = await helper.getContainerSizeAsync((int)containerTypeId);
                                    break;
                                case 15:
                                    cdValue.TcontainerDocValue = shippingline;
                                    break;
                                case 16:
                                    cdValue.TcontainerDocValue = bookingReference;
                                    break;
                                case 17:
                                    cdValue.TcontainerDocValue = shippingline;
                                    break;
                                default:
                                    cdValue.TcontainerDocValue = string.Empty;
                                    break;
                            }

                            await config.TLoadContainerDocValues.AddAsync(cdValue);
                            await config.SaveChangesAsync();
                        }

                        await transaction.CommitAsync();
                        blnStatus = true;
                    }
                    catch(Exception configE)
                    {
                        await transaction.RollbackAsync();
                        throw configE;
                    }
                }

                return blnStatus;
            }
            catch(Exception x)
            {
                return blnStatus;
            }
        }

        private async Task<decimal> getContainerVolumeAsync()
        {
            //gets the volume of the container
            decimal results = 0m;

            try
            {
                using (var config = new swContext())
                {
                    var obj = await config.TcontainerTypes.Where(x => x.Id == containerTypeId).FirstOrDefaultAsync();
                    if (obj != null)
                    {
                        results = (decimal)obj.Cvolume;
                    }
                }

                return results;
            }
            catch(Exception x)
            {
                return results;
            }
        }

        public async Task<bool> recordExists()
        {
            //TODO: check if record does exists
            bool blnStatus = false;

            try
            {
                using (var config = new swContext())
                {
                    try
                    {
                        var obj = await config.TLoadContainers.Where(c => c.ContainerName == nameOfcontainer).FirstOrDefaultAsync();

                        blnStatus = obj != null ? true : false;
                    }
                    catch(Exception tEx)
                    {
                        throw tEx;
                    }
                }

                return blnStatus;
            }
            catch(Exception x)
            {
                return blnStatus;
            }
        }

        public async Task<bool> UpdateContainer()
        {
            //TODO: updates container record in the data store
            bool blnStatus = false;

            try
            {
                using (var config = new swContext())
                {
                    try
                    {
                        var containerRecord = await config.TLoadContainers.Where(c => c.ContainerName == nameOfcontainer).FirstOrDefaultAsync();
                        if (containerRecord != null)
                        {
                            //update
                            containerRecord.ContainerTypeId = containerTypeId;
                            containerRecord.ReceivingAgentId = receivingAgentId;
                            containerRecord.ConsignorAgentId = consignorAgentId;
                            containerRecord.BookingRef = bookingReference;
                            containerRecord.Freight = freight;
                            containerRecord.Haulage = haulage;
                            containerRecord.ContainerVolume = await getContainerVolumeAsync();
                            containerRecord.SealNo = sealNumber;
                            containerRecord.BookedwithShippingLineId = bookedWithShippingLineId;
                            containerRecord.Qty = quantity;
                            containerRecord.ContainerStatusId = (int) ContainerLoadingStatus.ContainerLoadingStatusEnum.NOT_COMPLETED;
                            containerRecord.ContainerCode = containerCode;

                            containerRecord.ScheduleId = scheduleID;

                            await config.SaveChangesAsync();

                            blnStatus = true;
                        }
                    }
                    catch(Exception tErr)
                    {
                        throw tErr;
                    }
                }

                return blnStatus;
            }
            catch(Exception x)
            {
                return blnStatus;
            }
        }
    
        public async Task<IEnumerable<clsContainerStatus>> fetchContainerStatesAsync()
        {
            //method fetches all the status for loading a container
            List<clsContainerStatus> results = null;

            try
            {
                using (var config = new swContext())
                {
                    try
                    {
                        var dta = await config.TLoadContainerStatuses.ToListAsync();
                        results = dta.Select(a => new clsContainerStatus()
                        {
                            id = a.Id,
                            describ = a.ContainerLoadingStatus
                        }).ToList();
                    }
                    catch(Exception cErr)
                    {
                        throw cErr;
                    }
                }

                return results.ToList();
            }
            catch(Exception x)
            {
                return results;
            }
        }

    }

    public class clsContainerStatus
    {
        public int id { get; set; }
        public string describ { get; set; }
    }
}
