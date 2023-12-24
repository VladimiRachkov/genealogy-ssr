using Genealogy.Repository.Abstract;
using Genealogy.Service.Astract;
using MintPlayer.AspNetCore.SpaServices.Prerendering.Services;
using MintPlayer.AspNetCore.SpaServices.Routing;

namespace genealogy_ssr.Services
{
	public class SpaPrerenderingService : ISpaPrerenderingService
	{
		#region Constructor
		private readonly ISpaRouteService spaRouteService;

		public SpaPrerenderingService(ISpaRouteService spaRouteService)
		{
			this.spaRouteService = spaRouteService;
		}
		#endregion

		public Task BuildRoutes(ISpaRouteBuilder routeBuilder)
		{
			routeBuilder
				.Route("", "home");

			return Task.CompletedTask;
		}

		public async Task OnSupplyData(HttpContext context, IDictionary<string, object> data)
		{
			var route = await spaRouteService.GetCurrentRoute(context);
			switch (route?.Name)
			{
				default:
					break;
			}
		}
	}
}