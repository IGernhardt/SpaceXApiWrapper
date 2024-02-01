using GraphQL;
using GraphQL.Client.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using SpaceXApiWrapper.Models;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Diagnostics;
using Azure;
using System.Net.Sockets;

namespace SpaceXApiWrapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        string base_url = "https://spacex-production.up.railway.app/";
        string endpoint = "graphQL";
        string uri;

        public EventController()
        {
            uri = base_url + endpoint;
        }

        public enum SortDirection 
        { 
            Ascending,
            Descending,
        }

        public enum CapsuleSortBy
        {
            None,
            Type,
            Status,
            ReuseCount,
        }
        public enum CapsuleFilterBy
        {
            None,
            Type,
            Status,
            ReuseCount,
        }

        public enum LaunchesSortBy
        {
            None,
            LaunchDate,
            MissionName,
            RocketName,
            Upcoming,
        }
        public enum LaunchesFilterBy
        {
            None,
            LaunchDate,
            MissionName,
            RocketName,
            Upcoming,
        }
        public enum LaunchPadsSortBy
        {
            None,
            Name,
            Status,
        }
        public enum LaunchPadsFilterBy
        {
            None,
            Name,
            Status,
        }

        [Route("capsules")]
        [HttpGet]
        public async Task<ActionResult<Capsules>> GetCapsules(
            int page = 1, 
            int pagesize = 150, 
            CapsuleSortBy sortMethod = CapsuleSortBy.None, 
            SortDirection sortDirection = SortDirection.Ascending,
            string filter = "",
            CapsuleFilterBy filterBy = CapsuleFilterBy.None)
        {
            string query =
                @"
                query Capsules {
                  capsules {
                    id
                    type
                    status
                    reuse_count
                  }
                }";
            try
            {

                using (var httpClient = new HttpClient())
                {
                    var graphQLHttpClientOptions = new GraphQLHttpClientOptions
                    {
                        EndPoint = new Uri(uri)
                    };

                    var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

                    var request = new GraphQLRequest()
                    {
                        Query = query,
                    };

                    var response = await graphQLClient.SendQueryAsync<Capsules>(request);
                    var result = response.Data;

                    if (result == null)
                    {
                        return BadRequest(result);
                    }
                    // Filter
                    filter = filter.ToLower();
                    List<Capsule> treatedList = result.capsules
                        .Where(x =>
                        {
                            switch (filterBy)
                            {
                                case CapsuleFilterBy.Type:
                                    return x.type.ToLower().Contains(filter);
                                case CapsuleFilterBy.Status:
                                    return x.status.ToLower().Contains(filter);
                                case CapsuleFilterBy.ReuseCount:
                                    return x.reuse_count.ToString().ToLower() == filter;
                                default:
                                    return true;
                            }
                        }).ToList();
                    // Sort
                    if (sortMethod != CapsuleSortBy.None)
                    {
                        if (sortDirection == SortDirection.Descending)
                        {
                            switch (sortMethod)
                            {
                                case CapsuleSortBy.Type:
                                    treatedList = treatedList.OrderByDescending(x => x.type).ToList();
                                    break;
                                case CapsuleSortBy.Status:
                                    treatedList = treatedList.OrderByDescending(x => x.status).ToList();
                                    break;
                                case CapsuleSortBy.ReuseCount:
                                    treatedList = treatedList.OrderByDescending(x => x.reuse_count).ToList();
                                    break;
                                default:
                                    break;
                            }
                        } else
                        {
                            switch (sortMethod)
                            {
                                case CapsuleSortBy.Type:
                                    treatedList = treatedList.OrderBy(x => x.type).ToList();
                                    break;
                                case CapsuleSortBy.Status:
                                    treatedList = treatedList.OrderBy(x => x.status).ToList();
                                    break;
                                case CapsuleSortBy.ReuseCount:
                                    treatedList = treatedList.OrderBy(x => x.reuse_count).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    // Paginate
                    treatedList = treatedList
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

                    result = new Capsules() { capsules = treatedList };

                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("launches")]
        [HttpGet]
        public async Task<ActionResult<Launches>> GetLaunches(
            int page = 1,
            int pagesize = 150,
            LaunchesSortBy sortMethod = LaunchesSortBy.None,
            SortDirection sortDirection = SortDirection.Ascending,
            string filter = "",
            LaunchesFilterBy filterBy = LaunchesFilterBy.None)
        {
            string query =
                @"
                query Launches {
                  launches {
                    id
                    details
                    launch_date_utc
                    mission_id
                    mission_name
                    rocket {
                      rocket {
                        id
                        active
                        boosters
                        company
                        cost_per_launch
                        country
                        description
                        first_stage {
                          thrust_vacuum {
                            kN
                          }
                          thrust_sea_level {
                            kN
                          }
                          reusable
                          fuel_amount_tons
                          engines
                          burn_time_sec
                        }
                        landing_legs {
                          number
                        }
                        mass {
                          kg
                        }
                        name
                        payload_weights {
                          kg
                          name
                          id
                        }
                        second_stage {
                          burn_time_sec
                          engines
                          fuel_amount_tons
                          thrust {
                            kN
                          }
                        }
                        stages
                        success_rate_pct
                        type
                      }
                    }
                    upcoming
                  }
                }";
            try
            {

                using (var httpClient = new HttpClient())
                {
                    var graphQLHttpClientOptions = new GraphQLHttpClientOptions
                    {
                        EndPoint = new Uri(uri)
                    };

                    var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

                    var request = new GraphQLRequest()
                    {
                        Query = query,
                    };

                    var response = await graphQLClient.SendQueryAsync<Launches>(request);
                    var result = response.Data;

                    if (result == null)
                    {
                        return BadRequest(result);
                    }
                    // Filter
                    filter = filter.ToLower();
                    List<Launch> treatedList = result.launches
                        .Where(x =>
                        {
                            switch (filterBy)
                            {
                                case LaunchesFilterBy.LaunchDate:
                                    return x.launch_date_utc.ToString().ToLower().Contains(filter);
                                case LaunchesFilterBy.MissionName:
                                    return x.mission_name.ToLower().Contains(filter);
                                case LaunchesFilterBy.RocketName:
                                    return x.rocket.rocket.name.ToLower().Contains(filter);
                                case LaunchesFilterBy.Upcoming:
                                    return x.upcoming.ToString().ToLower() == filter;
                                default:
                                    return true;
                            }
                        }).ToList();
                    // Sort
                    if (sortMethod != LaunchesSortBy.None)
                    {
                        if (sortDirection == SortDirection.Descending)
                        {
                            switch (sortMethod)
                            {
                                case LaunchesSortBy.LaunchDate:
                                    treatedList = treatedList.OrderByDescending(x => x.launch_date_utc).ToList();
                                    break;
                                case LaunchesSortBy.MissionName:
                                    treatedList = treatedList.OrderByDescending(x => x.mission_name).ToList();
                                    break;
                                case LaunchesSortBy.RocketName:
                                    treatedList = treatedList.OrderByDescending(x => x.rocket.rocket.name).ToList();
                                    break;
                                case LaunchesSortBy.Upcoming:
                                    treatedList = treatedList.OrderByDescending(x => x.upcoming).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (sortMethod)
                            {
                                case LaunchesSortBy.LaunchDate:
                                    treatedList = treatedList.OrderBy(x => x.launch_date_utc).ToList();
                                    break;
                                case LaunchesSortBy.MissionName:
                                    treatedList = treatedList.OrderBy(x => x.mission_name).ToList();
                                    break;
                                case LaunchesSortBy.RocketName:
                                    treatedList = treatedList.OrderBy(x => x.rocket.rocket.name).ToList();
                                    break;
                                case LaunchesSortBy.Upcoming:
                                    treatedList = treatedList.OrderBy(x => x.upcoming).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    // Paginate
                    treatedList = treatedList
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

                    result = new Launches() { launches = treatedList };

                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("launches-upcoming")]
        [HttpGet]
        public async Task<ActionResult<LaunchesUpcoming>> GetLaunchesUpcoming(
            int page = 1,
            int pagesize = 150,
            LaunchesSortBy sortMethod = LaunchesSortBy.None,
            SortDirection sortDirection = SortDirection.Ascending,
            string filter = "",
            LaunchesFilterBy filterBy = LaunchesFilterBy.None)
        {
            string query =
                @"
                query Launch {
                  launchesUpcoming {
                    id
                    details
                    launch_date_utc
                    mission_id
                    mission_name
                    rocket {
                      rocket {
                        id
                        active
                        boosters
                        company
                        cost_per_launch
                        country
                        description
                        first_stage {
                          thrust_vacuum {
                            kN
                          }
                          thrust_sea_level {
                            kN
                          }
                          reusable
                          fuel_amount_tons
                          engines
                          burn_time_sec
                        }
                        landing_legs {
                          number
                        }
                        mass {
                          kg
                        }
                        name
                        payload_weights {
                          kg
                          name
                          id
                        }
                        second_stage {
                          burn_time_sec
                          engines
                          fuel_amount_tons
                          thrust {
                            kN
                          }
                        }
                        stages
                        success_rate_pct
                        type
                      }
                    }
                    upcoming
                  }
                }";
            try
            {

                using (var httpClient = new HttpClient())
                {
                    var graphQLHttpClientOptions = new GraphQLHttpClientOptions
                    {
                        EndPoint = new Uri(uri)
                    };

                    var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

                    var request = new GraphQLRequest()
                    {
                        Query = query,
                    };

                    var response = await graphQLClient.SendQueryAsync<LaunchesUpcoming>(request);
                    var result = response.Data;

                    if (result == null)
                    {
                        return BadRequest(result);
                    }
                    // Filter
                    filter = filter.ToLower();
                    List<Launch> treatedList = result.launchesUpcoming
                        .Where(x =>
                        {
                            switch (filterBy)
                            {
                                case LaunchesFilterBy.LaunchDate:
                                    return x.launch_date_utc.ToString().ToLower().Contains(filter);
                                case LaunchesFilterBy.MissionName:
                                    return x.mission_name.ToLower().Contains(filter);
                                case LaunchesFilterBy.RocketName:
                                    return x.rocket.rocket.name.ToLower().Contains(filter);
                                case LaunchesFilterBy.Upcoming:
                                    return x.upcoming.ToString().ToLower() == filter;
                                default:
                                    return true;
                            }
                        }).ToList();
                    // Sort
                    if (sortMethod != LaunchesSortBy.None)
                    {
                        if (sortDirection == SortDirection.Descending)
                        {
                            switch (sortMethod)
                            {
                                case LaunchesSortBy.LaunchDate:
                                    treatedList = treatedList.OrderByDescending(x => x.launch_date_utc).ToList();
                                    break;
                                case LaunchesSortBy.MissionName:
                                    treatedList = treatedList.OrderByDescending(x => x.mission_name).ToList();
                                    break;
                                case LaunchesSortBy.RocketName:
                                    treatedList = treatedList.OrderByDescending(x => x.rocket.rocket.name).ToList();
                                    break;
                                case LaunchesSortBy.Upcoming:
                                    treatedList = treatedList.OrderByDescending(x => x.upcoming).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (sortMethod)
                            {
                                case LaunchesSortBy.LaunchDate:
                                    treatedList = treatedList.OrderBy(x => x.launch_date_utc).ToList();
                                    break;
                                case LaunchesSortBy.MissionName:
                                    treatedList = treatedList.OrderBy(x => x.mission_name).ToList();
                                    break;
                                case LaunchesSortBy.RocketName:
                                    treatedList = treatedList.OrderBy(x => x.rocket.rocket.name).ToList();
                                    break;
                                case LaunchesSortBy.Upcoming:
                                    treatedList = treatedList.OrderBy(x => x.upcoming).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    // Paginate
                    treatedList = treatedList
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

                    result = new LaunchesUpcoming() { launchesUpcoming = treatedList };

                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("launchpads")]
        [HttpGet]
        public async Task<ActionResult<LaunchPads>> GetLaunchPads(
            int page = 1,
            int pagesize = 150,
            LaunchPadsSortBy sortMethod = LaunchPadsSortBy.None,
            SortDirection sortDirection = SortDirection.Ascending,
            string filter = "",
            LaunchPadsFilterBy filterBy = LaunchPadsFilterBy.None)
        {
            string query =
                @"
                query Launchpads {
                  launchpads {
                    id
                    details
                    name
                    status
                    wikipedia
                  }
                }";
            try
            {

                using (var httpClient = new HttpClient())
                {
                    var graphQLHttpClientOptions = new GraphQLHttpClientOptions
                    {
                        EndPoint = new Uri(uri)
                    };

                    var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

                    var request = new GraphQLRequest()
                    {
                        Query = query,
                    };

                    var response = await graphQLClient.SendQueryAsync<LaunchPads>(request);
                    var result = response.Data;

                    if (result == null)
                    {
                        return BadRequest(result);
                    }
                    // Filter
                    filter = filter.ToLower();
                    List<LaunchPad> treatedList = result.launchpads
                        .Where(x =>
                        {
                            switch (filterBy)
                            {
                                case LaunchPadsFilterBy.Name:
                                    return x.name.ToLower().Contains(filter);
                                case LaunchPadsFilterBy.Status:
                                    return x.status.ToLower().Contains(filter);
                                default:
                                    return true;
                            }
                        }).ToList();
                    // Sort
                    if (sortMethod != LaunchPadsSortBy.None)
                    {
                        if (sortDirection == SortDirection.Descending)
                        {
                            switch (sortMethod)
                            {
                                case LaunchPadsSortBy.Name:
                                    treatedList = treatedList.OrderByDescending(x => x.name).ToList();
                                    break;
                                case LaunchPadsSortBy.Status:
                                    treatedList = treatedList.OrderByDescending(x => x.status).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (sortMethod)
                            {
                                case LaunchPadsSortBy.Name:
                                    treatedList = treatedList.OrderBy(x => x.name).ToList();
                                    break;
                                case LaunchPadsSortBy.Status:
                                    treatedList = treatedList.OrderBy(x => x.status).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    // Paginate
                    treatedList = treatedList
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

                    result = new LaunchPads() { launchpads = treatedList };

                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
