import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { RouteTable } from "@presentation/components/ui/Tables/RoutesTable/RouteTable";
import "./RoutesPage.css";

export const RoutesPage = memo(() => {
  return <Fragment>
    <Seo title="Transport Company | Routes" />
    <WebsiteLayout>
    <Box className="routes-page">
      <Box className="routes-page-content">
        <ContentCard>
          <RouteTable />
        </ContentCard>
      </Box>
    </Box>
    </WebsiteLayout>
  </Fragment>
});
