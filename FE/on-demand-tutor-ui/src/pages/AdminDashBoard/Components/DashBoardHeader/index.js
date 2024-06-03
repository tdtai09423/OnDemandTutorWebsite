import images from "../../../../assets/images";

function DashBoardHeader() {
    return (
        <header className="d-flex align-item-center">
            <div className="container-fluid w-1000">
                <div className="row d-flex align-item-center">
                    <div className="col-xs-3">
                        <img src={images.logo} alt="OnDemandTutorLogo" />
                    </div>
                </div>
            </div>
        </header>
    );
}

export default DashBoardHeader;