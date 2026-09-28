import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { BookingTable } from "@presentation/components/ui/Tables/BookingsTable/BookingTable";
import "./BookingsPage.css";

export const BookingsPage = memo(() => {
  return <Fragment>
    <Seo title="Transport Company | Bookings" />
    <WebsiteLayout>
    <Box className="bookings-page">
        <Box className="bookings-page-content">
          <ContentCard>
            <BookingTable />
          </ContentCard>
        </Box>
    </Box>
    </WebsiteLayout>
  </Fragment>
});
