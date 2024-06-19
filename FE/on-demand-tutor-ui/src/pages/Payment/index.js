import React from "react";
import { useRef } from "react";
import emailjs from "@emailjs/browser";
import { Link, useNavigate } from "react-router-dom";
import { usePaymentInputs } from "react-payment-inputs";
import OrderInfo from "./components/order-info";
import './style.scss'
import { Col, Row } from "react-bootstrap";
import { useSearchParams } from 'react-router-dom';

function Payment() {
    const [searchParam] = useSearchParams();
    const tutorId = searchParam.get('tutorId');
    const form = useRef();
    const navigate = useNavigate();
    const {
        meta,
        getCardNumberProps,
        getExpiryDateProps,
        getCVCProps
    } = usePaymentInputs();

    const [checked, setChecked] = React.useState(true);
    const [cardNumber, setCardNumber] = React.useState("");
    const [details, setDetails] = React.useState({
        expiryDate: "",
        cvc: "",
        NomDuClient: ""
    });

    const handleChange = (e) => {
        setDetails((prevFormDetails) => {
            return {
                ...prevFormDetails,
                [e.target.name]: e.target.value
            };
        });

        console.log(details);
    };
    const handleChangeCardNumber = (e) => {
        setCardNumber(
            e.target.value
                .replace(/[^\dA-Z]/g, "")
                .replace(/(.{4})/g, "$1 ")
                .trim()
        );
    };
    const handleSubmit = (e) => {
        e.preventDefault();
        if (
            (meta.isTouched && meta.error) ||
            Number(cardNumber.length) < 19 ||
            cardNumber.trim().length === 0 ||
            details.expiryDate.trim().length === 0 ||
            details.cvc.trim().length === 0 ||
            details.NomDuClient.trim().length === 0 ||
            checked === true
        ) {
            setChecked(true);
            console.log("not submit");
        } else {
            setChecked(false);

            emailjs
                .sendForm(
                    "service_pduy8oo",
                    "template_be4vpep",
                    form.current,
                    "d7GFUxt5sOvLttX-o"
                )
                .then(
                    (result) => {
                        console.log(result.text);
                    },
                    (error) => {
                        console.log(error.text);
                    }
                );
            navigate("/Validation");
        }
    };
    const handleCheck = () => {
        console.log("ok");

        setChecked(false);
    };

    return (

        <Row>
            <Col md={7}>
                <OrderInfo tutorId={tutorId} />
            </Col>
            <Col md={3}>
                <form ref={form} className="form" onSubmit={handleSubmit}>
                    <header>
                        <div className="TitleSecure">
                            <h3>Paiement Details </h3>
                        </div>
                        <div className="Amont">
                            <p> Amount : </p>
                            <label className="price">100$</label>
                        </div>
                    </header>
                    <main>
                        {meta.isTouched && meta.error ? (
                            <span className="span">Error: {meta.error}</span>
                        ) : (
                            <span className="span"></span>
                        )}
                        <div className="NomDuClient">
                            <label> Nom du Client </label>
                            <input name="NomDuClient" onChange={handleChange} />
                        </div>
                        <div className="NumDeCarte">
                            <label> Num de Carte </label>
                            <input
                                // {...getCardNumberProps({ onChange: handleChangeCardNumber })}
                                onChange={handleChangeCardNumber}
                                placeholder="Valid Card Number"
                                name="cardNumber"
                                maxLength="19"
                                value={cardNumber}
                            />
                        </div>
                        <div className="DateEtCvc">
                            <div className="Date">
                                <label> Dte d'expiration </label>
                                <input
                                    {...getExpiryDateProps({ onChange: handleChange })}
                                    placeholder="MM/AA"
                                    name="expiryDate"
                                />
                            </div>
                            <div className="CvC">
                                <label> CvC</label>
                                <input
                                    {...getCVCProps({ onChange: handleChange })}
                                    name="cvc"
                                    maxLength="3"
                                />
                            </div>
                        </div>
                        <div className="" style={{ marginTop: '15px' }}>
                            By pressing the "Confirm payment" button, you agree to our Policy
                            It’s safe to pay on Preply. All transactions are protected by SSL encryption.
                        </div>
                        <input
                            disabled={checked}
                            type="submit"
                            value="Valider"
                            className="btn"
                        />
                    </main>
                    <footer>
                        <img className="img1" src="/images/methode.jpg" alt="" />
                        <img className="img2" src="/images/mir.png" alt="" />
                    </footer>
                </form>
            </Col>
            <Col md={2}>
            </Col>
        </Row>
    );
}

export default Payment
