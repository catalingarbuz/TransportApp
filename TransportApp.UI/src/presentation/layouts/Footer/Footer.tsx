import { FC } from "react";
import "./footer.scss";

/**
 * Here we have a simple footer container that will stay on the bottom of the page.
 */
export const Footer: FC<{}> = () => {
  const year = new Date().getFullYear();

  return (
    <div className="container">
      <footer className="py-3 my-4">
        <p className="nav justify-content-center border-bottom pb-3 mb-3 footer-divider">
        </p>
        <p className="text-center text-body-primary">
          © {year} Transport Company, Inc
        </p>
      </footer>
    </div>
  );
};
