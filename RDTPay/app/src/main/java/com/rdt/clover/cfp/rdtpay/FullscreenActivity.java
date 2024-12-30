package com.rdt.clover.cfp.rdtpay;

import android.annotation.SuppressLint;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;
import com.clover.cfp.activity.helper.CloverCFPActivityHelper;
import com.clover.cfp.activity.CloverCFPActivity;

/**
 * An example full-screen activity that shows and hides the system UI (i.e.
 * status bar and navigation/system bar) with user interaction.
 */
public class FullscreenActivity extends CloverCFPActivity  {
    /**
     * Some older devices needs a small delay between UI widget updates
     * and a change of the status and navigation bar.
     */
    private CloverCFPActivityHelper activityHelper;
    private static final int UI_ANIMATION_DELAY = 300;
    private final Handler mHideHandler = new Handler();
    private final Runnable mHidePart2Runnable = new Runnable() {
        @SuppressLint("InlinedApi")
        @Override
        public void run() {
            // Delayed removal of status and navigation bar
            View ll = findViewById(R.id.fullscreen_content_controls);
            // Note that some of these constants are new as of API 16 (Jelly Bean)
            // and API 19 (KitKat). It is safe to use them, as they are inlined
            // at compile-time and do nothing on earlier devices.
            ll.setSystemUiVisibility(View.SYSTEM_UI_FLAG_LOW_PROFILE
                    | View.SYSTEM_UI_FLAG_FULLSCREEN
                    | View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                    | View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY
                    | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                    | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION);
        }
    };

    private final Runnable mHideRunnable = new Runnable() {
        @Override
        public void run() {
            hide();
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_fullscreen);

        activityHelper = new CloverCFPActivityHelper(this);

        Button btnCash = findViewById(R.id.btnCash);
        String payload = activityHelper.getInitialPayload();
        Boolean payLoadEmpty = (payload == null || payload == "");

        Integer difference = payload.compareTo(RefundTypes.CashAndCard.toString());

        if (!payLoadEmpty && payload.compareTo(RefundTypes.Cash.toString()) != 0)
        {
            btnCash.setEnabled(false);
        }

        btnCash.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                setResultAndFinish(RESULT_OK, RefundTypes.Cash.toString());
            }
        });

        Button btnCashCard = findViewById(R.id.btnCashCard);

        if (!payLoadEmpty && payload.compareTo(RefundTypes.CashAndCard.toString()) != 0)
        {
            btnCashCard.setEnabled(false);
        }

        btnCashCard.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                setResultAndFinish(RESULT_OK,RefundTypes.CashAndCard.toString());
            }
        });

        Button btnCard = findViewById(R.id.btnCard);

        if (!payLoadEmpty && payload.compareTo(RefundTypes.Card.toString()) != 0)
        {
            btnCard.setEnabled(false);
        }

        btnCard.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                setResultAndFinish(RESULT_OK,RefundTypes.Card.toString());
            }
        });

        Button btnEnable = findViewById(R.id.btnEnable);

        btnEnable.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                btnCash.setEnabled(true);
                btnCashCard.setEnabled(true);
                btnCard.setEnabled(true);
            }
        });
    }

    @Override
    protected void onPostCreate(Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);

        delayedHide(100);
    }

    private void hide() {
        mHideHandler.postDelayed(mHidePart2Runnable, UI_ANIMATION_DELAY);
    }

    private void delayedHide(int delayMillis) {
        mHideHandler.removeCallbacks(mHideRunnable);
        mHideHandler.postDelayed(mHideRunnable, delayMillis);
    }
}