import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { RouteTable } from "@presentation/components/ui/Tables/RoutesTable/RouteTable";

export const RoutesPage = memo(() => {
  return <Fragment>
    <Seo title="MobyLab Web App | Routes" />
    <WebsiteLayout>
    <Box sx={{
          position: "fixed",
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          padding: "0px 300px 00px 300px",
          justifyItems: "center",
          height: "100vh",
          width: "100%",
          backgroundImage: "url('src/presentation/assets/img/background3.jpg')",
          backgroundSize: "cover",
          backgroundPosition: "center"}}>
      <Box sx={{ padding: "0px 50px 00px 50px", justifyItems: "center" }}>
        <ContentCard>
          <RouteTable />
        </ContentCard>
      </Box>
    </Box>
    </WebsiteLayout>
  </Fragment>
});
