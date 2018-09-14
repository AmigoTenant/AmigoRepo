using IdentityServer3.Core;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ViewService.Extensions
{
    public static class EmbeddedFileSystemExtensions
    {
        public static IAppBuilder UseAmigoTenantEmbeddedFileServer(this IAppBuilder app)
        {            
            app.UseFileServer(new FileServerOptions
            {
                RequestPath = new PathString("/content"),
                FileSystem = new EmbeddedResourceFileSystem(typeof(AmigoTenantViewService).Assembly, AssetManager.HttpAssetsNamespace)                
            });

            app.UseStageMarker(PipelineStage.MapHandler);

            app.UseFileServer(new FileServerOptions
            {
                RequestPath = new PathString("/content/fonts"),
                FileSystem = new EmbeddedResourceFileSystem(typeof(AmigoTenantViewService).Assembly, AssetManager.FontAssetsNamespace)
            });
            app.UseStageMarker(PipelineStage.MapHandler);            

            return app;
        }
    }
}