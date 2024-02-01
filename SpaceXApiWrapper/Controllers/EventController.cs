using GraphQL;
using GraphQL.Client.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using SpaceXApiWrapper.Models;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Diagnostics;

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

        [Route("capsules")]
        [HttpGet]
        public async Task<ActionResult<Capsules>> GetCapsules()
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
        public async Task<ActionResult<Launches>> GetLaunches()
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
        public async Task<ActionResult<LaunchesUpcoming>> GetLaunchesUpcoming()
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
        public async Task<ActionResult<LaunchPads>> GetLaunchPads()
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
