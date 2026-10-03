import { UserRoleEnum } from "@infrastructure/apis/client";
import { useOwnUserHasRole } from "@infrastructure/hooks/useOwnUser";
import { AppIntlProvider } from "@presentation/components/ui/AppIntlProvider";
import { ToastNotifier } from "@presentation/components/ui/ToastNotifier";
import { BookingsPage } from "@presentation/pages/BookingsPage";
import { HomePage } from "@presentation/pages/HomePage";
import { LoginPage } from "@presentation/pages/LoginPage";
import { RoutesPage } from "@presentation/pages/RoutesPage";
import { ContactPage } from "@presentation/pages/ContactPage";
import { SignupPage } from "@presentation/pages/SignupPage";
import { UserFilesPage } from "@presentation/pages/UserFilesPage";
import { UsersPage } from "@presentation/pages/UsersPage";
import { CarsPage } from "@presentation/pages/CarsPage";
import { DriversPage } from "@presentation/pages/DriversPage";
import { Route, Routes } from "react-router-dom";
import { AppRoute } from "routes";

export function App() {
  const isAdmin = useOwnUserHasRole(UserRoleEnum.Admin);

  return <AppIntlProvider> {/* AppIntlProvider provides the functions to search the text after the provides string ids. */}
      <ToastNotifier />
      {/* This adds the routes and route mappings on the various components. */}
      <Routes>
        <Route path={AppRoute.Index} element={<HomePage />} /> {/* Add a new route with a element as the page. */}
        <Route path={AppRoute.Login} element={<LoginPage />} />
        <Route path={AppRoute.Signup} element={<SignupPage />} />
        <Route path={AppRoute.Bookings} element={<BookingsPage />} />
        <Route path={AppRoute.Contact} element={<ContactPage/>} />
        {isAdmin && <Route path={AppRoute.Routes} element={<RoutesPage />} />} {/* If the user doesn't have the right role this route shouldn't be used. */}
        {isAdmin && <Route path={AppRoute.Users} element={<UsersPage />} />} 
        {isAdmin && <Route path={AppRoute.UserFiles} element={<UserFilesPage />} />}
        {isAdmin && <Route path={AppRoute.Cars} element={<CarsPage />} />}
        {isAdmin && <Route path={AppRoute.Drivers} element={<DriversPage />} />}
      </Routes>
    </AppIntlProvider>
}
